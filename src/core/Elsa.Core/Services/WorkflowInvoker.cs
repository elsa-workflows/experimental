using System;
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
        private readonly IWorkflowStateService _workflowStateService;
        private readonly IIdentityGraphService _identityGraphService;
        private readonly IActivitySchedulerFactory _schedulerFactory;

        public WorkflowInvoker(
            IServiceScopeFactory serviceScopeFactory,
            IActivityWalker activityWalker,
            IWorkflowExecutionPipeline pipeline,
            IWorkflowStateService workflowStateService,
            IIdentityGraphService identityGraphService,
            IActivitySchedulerFactory schedulerFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _activityWalker = activityWalker;
            _pipeline = pipeline;
            _workflowStateService = workflowStateService;
            _identityGraphService = identityGraphService;
            _schedulerFactory = schedulerFactory;
        }

        public async Task<WorkflowExecutionResult> ResumeAsync(Workflow workflow, Bookmark bookmark, WorkflowState workflowState, CancellationToken cancellationToken = default)
        {
            // Create a child scope.
            using var scope = _serviceScopeFactory.CreateScope();

            // Create workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, workflow, workflowState, default, bookmark, default, cancellationToken);

            // Construct bookmark.
            var activityDriverActivator = workflowExecutionContext.GetRequiredService<IActivityDriverActivator>();
            var bookmarkedActivity = workflowExecutionContext.FindActivityById(bookmark.ActivityId);
            var bookmarkedActivityDriver = activityDriverActivator.ActivateDriver(bookmarkedActivity);
            var resumeDelegate = bookmark.CallbackMethodName != null ? bookmarkedActivityDriver?.GetResumeActivityDelegate(bookmark.CallbackMethodName) : default;

            // Schedule the activity to resume.
            workflowExecutionContext.Scheduler.Push(new ScheduledActivity(bookmarkedActivity));

            // If no resumption point was specified, use Noop to prevent the regular "ExecuteAsync" method to be invoked.
            workflowExecutionContext.ExecuteDelegate = resumeDelegate ?? Noop;

            return await InvokeAsync(workflowExecutionContext);
        }

        public async Task<WorkflowExecutionResult> TriggerAsync(Workflow workflow, Trigger trigger, CancellationToken cancellationToken = default)
        {
            // Create a child scope.
            using var scope = _serviceScopeFactory.CreateScope();

            // Create workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, workflow, default, trigger, default, default, cancellationToken);

            // Get the activity to execute.
            var activity = workflowExecutionContext.FindActivityById(trigger.ActivityId);

            // Schedule the activity to execute.
            workflowExecutionContext.Scheduler.Push(new ScheduledActivity(activity));

            // Execute the workflow.
            return await InvokeAsync(workflowExecutionContext);
        }

        public async Task<WorkflowExecutionResult> InvokeAsync(Workflow workflow, CancellationToken cancellationToken = default)
        {
            // Create a child scope.
            using var scope = _serviceScopeFactory.CreateScope();

            // Setup a workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, workflow, default, default, default, default, cancellationToken);

            // Schedule the first node.
            var scheduledActivity = new ScheduledActivity(workflow.Root);
            workflowExecutionContext.Scheduler.Push(scheduledActivity);

            return await InvokeAsync(workflowExecutionContext);
        }

        private async Task<WorkflowExecutionResult> InvokeAsync(WorkflowExecutionContext workflowExecutionContext, WorkflowMiddlewareDelegate? pipeline = default)
        {
            pipeline ??= _pipeline.Pipeline;

            // Execute the activity execution pipeline.
            await pipeline(workflowExecutionContext);

            // Extract workflow state.
            var workflowState = _workflowStateService.ReadState(workflowExecutionContext);

            // Return workflow execution result containing state + bookmarks.
            return new WorkflowExecutionResult(workflowState, workflowExecutionContext.Bookmarks);
        }

        private WorkflowExecutionContext CreateWorkflowExecutionContext(
            IServiceProvider serviceProvider,
            Workflow workflow,
            WorkflowState? workflowState,
            Trigger? trigger,
            Bookmark? bookmark,
            ExecuteActivityDelegate? executeActivityDelegate,
            CancellationToken cancellationToken)
        {
            var root = workflow.Root;

            // Build graph.
            var graph = _activityWalker.Walk(root);

            // Assign identities.
            _identityGraphService.AssignIdentities(root);

            // Create scheduler.
            var scheduler = _schedulerFactory.CreateScheduler();

            // Setup a workflow execution context.
            var workflowExecutionContext = new WorkflowExecutionContext(serviceProvider, workflow, graph, scheduler, trigger, bookmark, executeActivityDelegate, cancellationToken);

            // Restore workflow execution context from state, if provided.
            if (workflowState != null)
            {
                var workflowStateService = serviceProvider.GetRequiredService<IWorkflowStateService>();
                workflowStateService.WriteState(workflowExecutionContext, workflowState);
            }

            return workflowExecutionContext;
        }
    }
}