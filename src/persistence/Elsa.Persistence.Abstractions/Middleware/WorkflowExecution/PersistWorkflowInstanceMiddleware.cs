using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Pipelines.WorkflowExecution;
using Elsa.State;

namespace Elsa.Persistence.Abstractions.Middleware.WorkflowExecution
{
    public static class PersistWorkflowInstanceMiddlewareExtensions
    {
        public static IWorkflowExecutionBuilder UsePersistWorkflowInstance(this IWorkflowExecutionBuilder builder) => builder.UseMiddleware<PersistWorkflowInstanceMiddleware>();
    }
    
    public class PersistWorkflowInstanceMiddleware
    {
        private readonly WorkflowMiddlewareDelegate _next;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowStateService _workflowStateService;
        private readonly IWorkflowBookmarkStore _workflowBookmarkStore;

        public PersistWorkflowInstanceMiddleware(WorkflowMiddlewareDelegate next, IWorkflowInstanceStore workflowInstanceStore, IWorkflowStateService workflowStateService, IWorkflowBookmarkStore workflowBookmarkStore)
        {
            _next = next;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowStateService = workflowStateService;
            _workflowBookmarkStore = workflowBookmarkStore;
        }

        public async ValueTask InvokeAsync(WorkflowExecutionContext context)
        {
            // Setup a new workflow instance.
            var workflowInstance = new WorkflowInstance
            {
                Id = context.Id,
                DefinitionId = context.Workflow.Id,
                Version = context.Workflow.Version,
                WorkflowState = _workflowStateService.ReadState(context)
            };
            
            context.SetProperty(nameof(WorkflowInstance), workflowInstance);
            
            // Persist workflow instance.
            await _workflowInstanceStore.SaveAsync(workflowInstance, context.CancellationToken);

            // Invoke next middleware.
            await _next(context);
            
            // Update workflow instance.
            workflowInstance.WorkflowState = _workflowStateService.ReadState(context);
            
            // Persist workflow instance.
            await _workflowInstanceStore.SaveAsync(workflowInstance, context.CancellationToken);
            
            // Delete any bookmark that was used to resume the workflow.
            if (context.Bookmark != null) 
                await _workflowBookmarkStore.DeleteAsync(context.Bookmark.Id, context.CancellationToken);

            // Persist bookmarks.
            var workflowBookmarks = context.Bookmarks.Select(x => new WorkflowBookmark
            {
                Id = x.Id,
                WorkflowDefinitionId = context.Workflow.Id,
                WorkflowInstanceId = workflowInstance.Id,
                Hash = x.Hash,
                Data = x.Data,
                Name = x.Name,
                ActivityId = x.ActivityId,
                CallbackMethodName = x.CallbackMethodName
            }).ToList();
            
            await _workflowBookmarkStore.SaveManyAsync(workflowBookmarks, context.CancellationToken);
        }
    }
}