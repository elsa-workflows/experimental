using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Services
{
    public class ActivityInvoker : IActivityInvoker
    {
        private static ValueTask Noop(ActivityExecutionContext context) => new();
        
        private readonly INodeExecutionPipeline _pipeline;
        private readonly IActivitySchedulerFactory _schedulerFactory;
        private readonly IIdentityGraphService _identityGraphService;
        private readonly IWorkflowStateService _workflowStateService;
        private readonly IActivityWalker _activityWalker;

        public ActivityInvoker(INodeExecutionPipeline pipeline, IActivitySchedulerFactory schedulerFactory, IIdentityGraphService identityGraphService, IWorkflowStateService workflowStateService, IActivityWalker activityWalker)
        {
            _pipeline = pipeline;
            _schedulerFactory = schedulerFactory;
            _identityGraphService = identityGraphService;
            _workflowStateService = workflowStateService;
            _activityWalker = activityWalker;
        }

        public async Task<WorkflowExecutionContext> ResumeAsync(string bookmarkName, IActivity root, WorkflowState workflowState, CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = CreateWorkflowExecutionContext(root, workflowState);
            var bookmark = workflowExecutionContext.PopBookmark(bookmarkName);

            if (bookmark == null)
                return workflowExecutionContext;
            
            // Schedule the node to resume.
            workflowExecutionContext.Scheduler.Schedule(bookmark.Target);

            // If no resumption point was specified, use Noop to prevent the regular "ExecuteAsync" method to be invoked.
            var executeDelegate = bookmark.Resume ?? Noop;
            return await InvokeAsync(workflowExecutionContext, executeDelegate, cancellationToken);
        }

        public async Task<WorkflowExecutionContext> InvokeAsync(IActivity activity, IActivity? root = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            var currentNode = new ScheduledActivity(activity);
            return await InvokeAsync(currentNode, root, null, executeNodeDelegate, cancellationToken);
        }

        private async Task<WorkflowExecutionContext> InvokeAsync(ScheduledActivity scheduledActivity, IActivity? root = default, WorkflowState? workflowState = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            // If no root was provided, it means the node *is* the root node.
            root ??=  scheduledActivity.Activity;
            
            // Setup a workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(root, workflowState);
            
            // Schedule the first node.
            workflowExecutionContext.Scheduler.Schedule(scheduledActivity);

            return await InvokeAsync(workflowExecutionContext, executeNodeDelegate, cancellationToken);
        }

        private WorkflowExecutionContext CreateWorkflowExecutionContext(IActivity root, WorkflowState? workflowState = default)
        {
            // Build graph.
            var graph = _activityWalker.Walk(root);
            
            // Assign identities.
            _identityGraphService.AssignIdentities(graph);

            // Create scheduler.
            var scheduler = _schedulerFactory.CreateScheduler();

            // Setup a workflow execution context.
            var workflowExecutionContext = new WorkflowExecutionContext(graph, scheduler);
            
            // Restore workflow execution context from state, if provided.
            if (workflowState != null)
                _workflowStateService.ApplyState(workflowExecutionContext, workflowState);

            return workflowExecutionContext;
        }

        private async Task<WorkflowExecutionContext> InvokeAsync(WorkflowExecutionContext workflowExecutionContext, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            var scheduler = workflowExecutionContext.Scheduler;
            
            // As long as there are nodes scheduled, keep executing them.
            while (scheduler.HasAny)
            {
                // Pop next node for execution.
                var currentNode = scheduler.Unschedule();

                // Setup a node execution context.
                var nodeExecutionContext = new ActivityExecutionContext(workflowExecutionContext, currentNode, executeNodeDelegate, cancellationToken);

                // Execute the node execution pipeline.
                await _pipeline.ExecuteAsync(nodeExecutionContext);

                // Reset custom node execution delegate. This is used only once for the initial node being executed.
                executeNodeDelegate = null;
            }

            return workflowExecutionContext;
        }
    }
}