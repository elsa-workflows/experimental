using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Extensions;
using Elsa.Runtime.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Services
{
    public class WorkflowManager : IWorkflowManager
    {
        private readonly IEnumerable<IWorkflowProvider> _workflowProviders;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowBookmarkStore _workflowBookmarkStore;
        private readonly IActivityInvoker _activityInvoker;
        private readonly ILogger<WorkflowManager> _logger;
        private readonly IWorkflowTriggerStore _workflowTriggerStore;

        public WorkflowManager(
            IEnumerable<IWorkflowProvider> workflowProviders,
            IWorkflowInstanceStore workflowInstanceStore,
            IWorkflowBookmarkStore workflowBookmarkStore,
            IWorkflowTriggerStore workflowTriggerStore,
            IActivityInvoker activityInvoker,
            ILogger<WorkflowManager> logger)
        {
            _workflowProviders = workflowProviders;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowBookmarkStore = workflowBookmarkStore;
            _activityInvoker = activityInvoker;
            _logger = logger;
            _workflowTriggerStore = workflowTriggerStore;
        }

        public async Task<Workflow?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            foreach (var workflowProvider in _workflowProviders)
            {
                var workflow = await workflowProvider.GetByIdAsync(id, cancellationToken);

                if (workflow != null)
                    return workflow;
            }

            return default!;
        }

        public async Task<PagedList<Workflow>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken)
        {
            var tasks = _workflowProviders.Select(x => x.ListAsync(pagerParameters, cancellationToken).AsTask());
            var workflows = (await Task.WhenAll(tasks)).SelectMany(x => x.Items);
            return workflows.Paginate(pagerParameters);
        }

        public async Task<IEnumerable<Workflow>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default) =>
            await FindManyByIdInternalAsync(ids, cancellationToken).ToListAsync(cancellationToken);

        public async Task<IEnumerable<WorkflowExecutionResult>> ResumeWorkflowsAsync(string bookmarkName, string? hash, CancellationToken cancellationToken = default)
        {
            var workflowBookmarks = (await _workflowBookmarkStore.FindManyAsync(bookmarkName, hash, cancellationToken)).ToList();
            var workflowDefinitionIds = workflowBookmarks.Select(x => x.WorkflowDefinitionId).Distinct().ToList();
            var workflowDefinitions = (await FindManyByIdAsync(workflowDefinitionIds, cancellationToken)).ToDictionary(x => x.Id);
            var results = new List<WorkflowExecutionResult>();
            var newBookmarkRecords = new List<WorkflowBookmark>();

            foreach (var workflowBookmark in workflowBookmarks)
            {
                var workflowInstanceId = workflowBookmark.WorkflowInstanceId;

                if (!workflowDefinitions.TryGetValue(workflowBookmark.WorkflowDefinitionId, out var workflowDefinition))
                {
                    _logger.LogWarning("Workflow bookmark {WorkflowBookmarkId} points to workflow definition ID {WorkflowDefinitionId}, bu no such workflow definition was found", workflowBookmark.Id, workflowBookmark.WorkflowDefinitionId);
                    continue;
                }

                var workflowInstance = await _workflowInstanceStore.GetByIdAsync(workflowInstanceId, cancellationToken);

                if (workflowInstance == null)
                {
                    _logger
                        .LogWarning(
                            "Workflow bookmark {WorkflowBookmarkId} for workflow definition {WorkflowDefinitionId} points to workflow instance ID {WorkflowInstanceId}, but no such workflow instance was found", workflowBookmark.Id, workflowBookmark.WorkflowDefinitionId, workflowBookmark.WorkflowInstanceId);

                    continue;
                }

                var bookmark = MapWorkflowBookmark(workflowBookmark);
                var result = await _activityInvoker.ResumeAsync(bookmark, workflowDefinition.Root, workflowInstance.WorkflowState, cancellationToken);

                results.Add(new WorkflowExecutionResult(workflowDefinition, workflowInstance));
                newBookmarkRecords.AddRange(result.Bookmarks.Select(x => MapBookmark(x, workflowDefinition.Id, workflowInstanceId!)));
            }

            // Delete used bookmarks.
            await _workflowBookmarkStore.DeleteManyAsync(workflowBookmarks.Select(x => x.Id), cancellationToken);

            // Store new bookmarks.
            await _workflowBookmarkStore.SaveManyAsync(newBookmarkRecords, cancellationToken);

            return results;
        }

        public async Task<IEnumerable<WorkflowExecutionResult>> TriggerWorkflowsAsync(string triggerName, string? hash, CancellationToken cancellationToken = default)
        {
            var workflowTriggers = (await _workflowTriggerStore.FindManyAsync(triggerName, hash, cancellationToken)).ToList();
            var workflowDefinitionIds = workflowTriggers.Select(x => x.WorkflowDefinitionId).Distinct().ToList();
            var workflowDefinitions = (await FindManyByIdAsync(workflowDefinitionIds, cancellationToken)).ToDictionary(x => x.Id);
            var results = new List<WorkflowExecutionResult>();
            var newWorkflowBookmarks = new List<WorkflowBookmark>();
            var workflowInstances = new List<WorkflowInstance>();

            foreach (var workflowTrigger in workflowTriggers)
            {
                if (!workflowDefinitions.TryGetValue(workflowTrigger.WorkflowDefinitionId, out var workflowDefinition))
                {
                    _logger.LogWarning("Workflow trigger {WorkflowTriggerId} points to workflow definition ID {WorkflowDefinitionId}, bu no such workflow definition was found", workflowTrigger.Id, workflowTrigger.WorkflowDefinitionId);
                    continue;
                }

                var trigger = MapWorkflowTrigger(workflowTrigger);
                var activityExecutionResult = await _activityInvoker.TriggerAsync(trigger, workflowDefinition.Root, cancellationToken);

                var workflowInstance = new WorkflowInstance
                {
                    Id = Guid.NewGuid().ToString("N"),
                    DefinitionId = workflowDefinition.Id,
                    Version = workflowDefinition.Version,
                    WorkflowState = activityExecutionResult.WorkflowState
                };

                var workflowExecutionResult = new WorkflowExecutionResult(workflowDefinition, workflowInstance);

                results.Add(workflowExecutionResult);
                workflowInstances.Add(workflowInstance);
                newWorkflowBookmarks.AddRange(activityExecutionResult.Bookmarks.Select(x => MapBookmark(x, workflowDefinition.Id, workflowInstance.Id)));
            }

            // Persist workflow instances.
            await _workflowInstanceStore.SaveManyAsync(workflowInstances, cancellationToken);

            // Store new bookmarks.
            await _workflowBookmarkStore.SaveManyAsync(newWorkflowBookmarks, cancellationToken);

            return results;
        }

        public async Task<WorkflowExecutionResult> ExecuteWorkflowAsync(Workflow workflowDefinition, CancellationToken cancellationToken = default)
        {
            var result = await _activityInvoker.InvokeAsync(workflowDefinition.Root, cancellationToken: cancellationToken);

            var workflowInstance = new WorkflowInstance
            {
                Id = Guid.NewGuid().ToString("N"),
                DefinitionId = workflowDefinition.Id,
                Version = workflowDefinition.Version,
                WorkflowState = result.WorkflowState
            };

            await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);
            var bookmarkRecords = result.Bookmarks.Select(x => MapBookmark(x, workflowDefinition.Id, workflowInstance.Id)).ToList();

            await _workflowBookmarkStore.SaveManyAsync(bookmarkRecords, cancellationToken);

            return new WorkflowExecutionResult(workflowDefinition, workflowInstance);
        }

        private async IAsyncEnumerable<Workflow> FindManyByIdInternalAsync(IEnumerable<string> ids, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var idList = ids as ICollection<string> ?? ids.ToHashSet();

            foreach (var workflowProvider in _workflowProviders)
            {
                var workflowDefinitions = await workflowProvider.FindManyByIdAsync(idList, cancellationToken);

                foreach (var workflowDefinition in workflowDefinitions)
                    yield return workflowDefinition;
            }
        }

        private Bookmark MapWorkflowBookmark(WorkflowBookmark workflowBookmark) => new Elsa.Models.Bookmark(workflowBookmark.Name, workflowBookmark.Hash, workflowBookmark.ActivityId, workflowBookmark.Data, workflowBookmark.CallbackMethodName);
        private Trigger MapWorkflowTrigger(WorkflowTrigger workflowTrigger) => new Trigger(workflowTrigger.Name, workflowTrigger.Hash, workflowTrigger.ActivityId, workflowTrigger.Data);

        private WorkflowBookmark MapBookmark(Bookmark bookmark, string workflowDefinitionId, string workflowInstanceId) => new()
        {
            Name = bookmark.Name,
            Hash = bookmark.Hash,
            ActivityId = bookmark.ActivityId,
            Data = bookmark.Data,
            WorkflowDefinitionId = workflowDefinitionId,
            WorkflowInstanceId = workflowInstanceId
        };
    }
}