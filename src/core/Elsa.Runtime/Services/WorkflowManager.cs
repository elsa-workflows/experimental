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
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Services
{
    public class WorkflowManager : IWorkflowManager
    {
        private readonly IEnumerable<IWorkflowProvider> _workflowProviders;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IBookmarkStore _bookmarkStore;
        private readonly IActivityInvoker _activityInvoker;

        public WorkflowManager(IEnumerable<IWorkflowProvider> workflowProviders, IWorkflowInstanceStore workflowInstanceStore, IBookmarkStore bookmarkStore, IActivityInvoker activityInvoker)
        {
            _workflowProviders = workflowProviders;
            _workflowInstanceStore = workflowInstanceStore;
            _bookmarkStore = bookmarkStore;
            _activityInvoker = activityInvoker;
        }

        public async Task<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            foreach (var workflowProvider in _workflowProviders)
            {
                var workflow = await workflowProvider.GetByIdAsync(id, cancellationToken);

                if (workflow != null)
                    return workflow;
            }

            return default!;
        }

        public async Task<IEnumerable<WorkflowDefinition>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default) =>
            await FindManyByIdInternalAsync(ids, cancellationToken).ToListAsync(cancellationToken);

        public async Task<IEnumerable<WorkflowExecutionResult>> ResumeBookmarksAsync(string bookmarkName, string hash, CancellationToken cancellationToken = default)
        {
            var bookmarkRecordList = (await _bookmarkStore.FindManyAsync(bookmarkName, hash, cancellationToken)).ToList();
            var workflowDefinitionIds = bookmarkRecordList.Select(x => x.WorkflowDefinitionId).Distinct().ToList();
            var workflowDefinitions = (await FindManyByIdAsync(workflowDefinitionIds, cancellationToken)).ToDictionary(x => x.Id);
            var results = new List<WorkflowExecutionResult>();
            var newBookmarkRecords = new List<BookmarkRecord>();

            foreach (var bookmarkRecord in bookmarkRecordList)
            {
                var workflowInstanceId = bookmarkRecord.WorkflowInstanceId;

                if (!workflowDefinitions.TryGetValue(bookmarkRecord.WorkflowDefinitionId, out var workflowDefinition))
                    continue;

                var workflowInstance = workflowInstanceId != null ? await _workflowInstanceStore.GetByIdAsync(workflowInstanceId, cancellationToken) : default;

                if (workflowInstance != null)
                {
                    var bookmark = MapBookmarkRecord(bookmarkRecord);
                    var result = await _activityInvoker.ResumeAsync(bookmark, workflowDefinition.Root, workflowInstance.WorkflowState, cancellationToken);

                    results.Add(result);
                    newBookmarkRecords.AddRange(result.Bookmarks.Select(x => MapBookmark(x, workflowDefinition.Id, workflowInstanceId!)));
                }
                else
                {
                    var result = await _activityInvoker.InvokeAsync(workflowDefinition.Root, cancellationToken: cancellationToken);
                    
                    workflowInstance = new WorkflowInstanceRecord
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        DefinitionId = workflowDefinition.Id,
                        Version = workflowDefinition.Version,
                        WorkflowState = result.WorkflowState
                    };

                    await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);
                    newBookmarkRecords.AddRange(result.Bookmarks.Select(x => MapBookmark(x, workflowDefinition.Id, workflowInstance.Id)));
                    results.Add(result);
                }
            }
            
            // Delete used bookmarks.
            await _bookmarkStore.DeleteManyAsync(bookmarkRecordList.Select(x => x.Id), cancellationToken);
            
            // Store new bookmarks.
            await _bookmarkStore.SaveManyAsync(newBookmarkRecords, cancellationToken);

            return results;
        }

        public async Task<WorkflowExecutionResult> ExecuteWorkflowAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default)
        {
            var result = await _activityInvoker.InvokeAsync(workflowDefinition.Root, cancellationToken: cancellationToken);
                    
            var workflowInstance = new WorkflowInstanceRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                DefinitionId = workflowDefinition.Id,
                Version = workflowDefinition.Version,
                WorkflowState = result.WorkflowState
            };

            await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);
            var bookmarkRecords = result.Bookmarks.Select(x => MapBookmark(x, workflowDefinition.Id, workflowInstance.Id)).ToList();

            await _bookmarkStore.SaveManyAsync(bookmarkRecords, cancellationToken);

            return result;
        }

        private async IAsyncEnumerable<WorkflowDefinition> FindManyByIdInternalAsync(IEnumerable<string> ids, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var idList = ids as ICollection<string> ?? ids.ToHashSet();

            foreach (var workflowProvider in _workflowProviders)
            {
                var workflowDefinitions = await workflowProvider.FindManyByIdAsync(idList, cancellationToken);

                foreach (var workflowDefinition in workflowDefinitions)
                    yield return workflowDefinition;
            }
        }

        private Bookmark MapBookmarkRecord(BookmarkRecord bookmarkRecord) => new Bookmark(bookmarkRecord.Name, bookmarkRecord.ActivityId, bookmarkRecord.Hash, bookmarkRecord.Data, bookmarkRecord.CallbackMethodName);
        
        private BookmarkRecord MapBookmark(Bookmark bookmark, string workflowDefinitionId, string workflowInstanceId) => new()
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