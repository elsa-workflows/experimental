using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Pipelines.WorkflowExecution;

namespace Elsa.Persistence.Abstractions.Middleware.WorkflowExecution
{
    public static class PersistWorkflowInstanceMiddlewareExtensions
    {
        public static IWorkflowExecutionBuilder PersistWorkflows(this IWorkflowExecutionBuilder builder) => builder.UseMiddleware<PersistWorkflowInstanceMiddleware>();
    }
    
    public class PersistWorkflowInstanceMiddleware : IWorkflowExecutionMiddleware
    {
        private readonly WorkflowMiddlewareDelegate _next;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowStateSerializer _workflowStateSerializer;
        private readonly IWorkflowBookmarkStore _workflowBookmarkStore;

        public PersistWorkflowInstanceMiddleware(WorkflowMiddlewareDelegate next, IWorkflowInstanceStore workflowInstanceStore, IWorkflowStateSerializer workflowStateSerializer, IWorkflowBookmarkStore workflowBookmarkStore)
        {
            _next = next;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowStateSerializer = workflowStateSerializer;
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
                WorkflowState = _workflowStateSerializer.ReadState(context)
            };

            var cancellationToken = context.CancellationToken;
            
            // Get a copy of current bookmarks.
            var existingBookmarks = await _workflowBookmarkStore.FindManyByWorkflowInstanceAsync(workflowInstance.Id, cancellationToken);
            var bookmarksSnapshot = existingBookmarks.Select(x => new Bookmark(x.Id, x.Name, x.Hash, x.ActivityId, x.Data, x.CallbackMethodName)).ToList();
            
            // Apply bookmarks to workflow context.
            context.RegisterBookmarks(bookmarksSnapshot);
            
            // Persist workflow instance.
            await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);

            // Invoke next middleware.
            await _next(context);
            
            // Update workflow instance.
            workflowInstance.WorkflowState = _workflowStateSerializer.ReadState(context);
            
            // Persist workflow instance.
            await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);
            
            // // Delete any bookmark that was used to resume the workflow.
            // if (context.Bookmark != null) 
            //     await _workflowBookmarkStore.DeleteAsync(context.Bookmark.Id, cancellationToken);
            
            // Remove bookmarks that were in the snapshot but no longer present in context.
            var removedBookmarks = bookmarksSnapshot.Except(context.Bookmarks).Select(x => x.Id).ToList();
            await _workflowBookmarkStore.DeleteManyAsync(removedBookmarks, cancellationToken);

            // Persist bookmarks, if any.
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