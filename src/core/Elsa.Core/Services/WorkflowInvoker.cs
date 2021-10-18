using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.State;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Services
{
    public class WorkflowInvoker : IWorkflowInvoker
    {
        private static ValueTask Noop(ActivityExecutionContext context) => new();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IActivityWalker _activityWalker;
        private readonly IWorkflowExecutionPipeline _pipeline;
        private readonly IWorkflowStateSerializer _workflowStateSerializer;
        private readonly IIdentityGraphService _identityGraphService;
        private readonly IActivitySchedulerFactory _schedulerFactory;

        public WorkflowInvoker(
            IServiceScopeFactory serviceScopeFactory,
            IActivityWalker activityWalker,
            IWorkflowExecutionPipeline pipeline,
            IWorkflowStateSerializer workflowStateSerializer,
            IIdentityGraphService identityGraphService,
            IActivitySchedulerFactory schedulerFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _activityWalker = activityWalker;
            _pipeline = pipeline;
            _workflowStateSerializer = workflowStateSerializer;
            _identityGraphService = identityGraphService;
            _schedulerFactory = schedulerFactory;
        }

        public async Task<WorkflowExecutionResult> InvokeAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default)
        {
            // Create a child scope.
            using var scope = _serviceScopeFactory.CreateScope();

            // Setup a workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, workflowDefinition, default, default, default, cancellationToken);

            // Schedule the first node.
            var activityInvoker = scope.ServiceProvider.GetRequiredService<IActivityInvoker>();
            var workflow = workflowDefinition.Workflow;
            var workItem = new ActivityWorkItem(workflow.Root.Id, async () => await activityInvoker.InvokeAsync(workflowExecutionContext, workflow.Root));
            workflowExecutionContext.Scheduler.Push(workItem);

            return await InvokeAsync(workflowExecutionContext);
        }

        public async Task<WorkflowExecutionResult> InvokeAsync(WorkflowDefinition workflowDefinition, WorkflowState workflowState, Bookmark? bookmark = default, CancellationToken cancellationToken = default)
        {
            // Create a child scope.
            using var scope = _serviceScopeFactory.CreateScope();

            // Create workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, workflowDefinition, workflowState, bookmark, default, cancellationToken);

            if (bookmark != null)
            {
                // Construct bookmark.
                var bookmarkedActivityContext = workflowExecutionContext.ActivityExecutionContexts.First(x => x.Id == bookmark.ActivityInstanceId);
                var bookmarkedActivity = bookmarkedActivityContext.Activity;

                // If no resumption point was specified, use Noop to prevent the regular "ExecuteAsync" method to be invoked.
                var resumeDelegate = bookmark.CallbackMethodName != null ? bookmarkedActivity.GetResumeActivityDelegate(bookmark.CallbackMethodName) : Noop;

                // Schedule the activity to resume.
                var activityInvoker = scope.ServiceProvider.GetRequiredService<IActivityInvoker>();
                var workItem = new ActivityWorkItem(bookmarkedActivity.Id, async () => await activityInvoker.InvokeAsync(bookmarkedActivityContext));
                workflowExecutionContext.Scheduler.Push(workItem);

                workflowExecutionContext.ExecuteDelegate = resumeDelegate;
            }

            return await InvokeAsync(workflowExecutionContext);
        }

        public async Task<WorkflowExecutionResult> InvokeAsync(WorkflowExecutionContext workflowExecutionContext)
        {
            // Execute the activity execution pipeline.
            await _pipeline.ExecuteAsync(workflowExecutionContext);

            // Extract workflow state.
            var workflowState = _workflowStateSerializer.ReadState(workflowExecutionContext);

            // Return workflow execution result containing state + bookmarks.
            return new WorkflowExecutionResult(workflowState, workflowExecutionContext.Bookmarks);
        }

        public WorkflowExecutionContext CreateWorkflowExecutionContext(
            IServiceProvider serviceProvider,
            WorkflowDefinition workflowDefinition,
            WorkflowState? workflowState,
            Bookmark? bookmark,
            ExecuteActivityDelegate? executeActivityDelegate,
            CancellationToken cancellationToken)
        {
            var root = workflowDefinition.Workflow.Root;

            // Build graph.
            var graph = _activityWalker.Walk(root);

            // Assign identities.
            _identityGraphService.AssignIdentities(graph);

            // Create scheduler.
            var scheduler = _schedulerFactory.CreateScheduler();

            // Setup a workflow execution context.
            var workflowExecutionContext = new WorkflowExecutionContext(serviceProvider, workflowDefinition, graph, scheduler, bookmark, executeActivityDelegate, cancellationToken);

            // Restore workflow execution context from state, if provided.
            if (workflowState != null)
            {
                var workflowStateService = serviceProvider.GetRequiredService<IWorkflowStateSerializer>();
                workflowStateService.WriteState(workflowExecutionContext, workflowState);
            }

            return workflowExecutionContext;
        }
    }
}