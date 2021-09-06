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
    public class ActivityInvoker : IActivityInvoker
    {
        private static ValueTask Noop(ActivityExecutionContext context) => new();

        private readonly IActivityExecutionPipeline _pipeline;
        private readonly IActivitySchedulerFactory _schedulerFactory;
        private readonly IIdentityGraphService _identityGraphService;
        private readonly IWorkflowStateService _workflowStateService;
        private readonly IActivityWalker _activityWalker;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ActivityInvoker(
            IActivityExecutionPipeline pipeline,
            IActivitySchedulerFactory schedulerFactory,
            IIdentityGraphService identityGraphService,
            IWorkflowStateService workflowStateService,
            IActivityWalker activityWalker,
            IServiceScopeFactory serviceScopeFactory)
        {
            _pipeline = pipeline;
            _schedulerFactory = schedulerFactory;
            _identityGraphService = identityGraphService;
            _workflowStateService = workflowStateService;
            _activityWalker = activityWalker;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<ActivityExecutionResult> ResumeAsync(Bookmark bookmark, IActivity root, WorkflowState workflowState, CancellationToken cancellationToken = default)
        {
            // Create a child service scope.
            using var scope = _serviceScopeFactory.CreateScope();
            
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, root, workflowState);
            
            // Construct bookmark.
            var activityDriverActivator = workflowExecutionContext.GetRequiredService<IActivityDriverActivator>();
            var bookmarkedActivity = workflowExecutionContext.FindActivityById(bookmark.ActivityId);
            var bookmarkedActivityDriver = activityDriverActivator.ActivateDriver(bookmarkedActivity);
            var resumeDelegate = bookmark.CallbackMethodName != null ? bookmarkedActivityDriver?.GetResumeActivityDelegate(bookmark.CallbackMethodName) : default;

            // Schedule the activity to resume.
            workflowExecutionContext.Scheduler.Schedule(new ScheduledActivity(bookmarkedActivity));

            // If no resumption point was specified, use Noop to prevent the regular "ExecuteAsync" method to be invoked.
            var executeDelegate = resumeDelegate ?? Noop;
            return await InvokeAsync(workflowExecutionContext, executeDelegate, cancellationToken);
        }
        
        public async Task<ActivityExecutionResult> TriggerAsync(Trigger trigger, IActivity root, CancellationToken cancellationToken = default)
        {
            // Create a child service scope.
            using var scope = _serviceScopeFactory.CreateScope();
            
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, root, trigger: trigger);
            
            // Get the activity to execute.
            var activity = workflowExecutionContext.FindActivityById(trigger.ActivityId);

            // Schedule the activity to execute.
            workflowExecutionContext.Scheduler.Schedule(new ScheduledActivity(activity));
            
            // Execute the workflow.
            return await InvokeAsync(workflowExecutionContext, cancellationToken: cancellationToken);
        }

        public async Task<ActivityExecutionResult> InvokeAsync(IActivity activity, IActivity? root = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            var currentNode = new ScheduledActivity(activity);
            return await InvokeAsync(currentNode, root, null, executeNodeDelegate, cancellationToken);
        }

        private async Task<ActivityExecutionResult> InvokeAsync(ScheduledActivity scheduledActivity, IActivity? root = default, WorkflowState? workflowState = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            // If no root was provided, it means the activity *is* the root activity.
            root ??= scheduledActivity.Activity;

            // Create a child service scope.
            using var scope = _serviceScopeFactory.CreateScope();

            // Setup a workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(scope.ServiceProvider, root, workflowState);

            // Schedule the first node.
            workflowExecutionContext.Scheduler.Schedule(scheduledActivity);

            return await InvokeAsync(workflowExecutionContext, executeNodeDelegate, cancellationToken);
        }

        private async Task<ActivityExecutionResult> InvokeAsync(WorkflowExecutionContext workflowExecutionContext, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
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

            var workflowState = _workflowStateService.CreateState(workflowExecutionContext);
            return new ActivityExecutionResult(workflowState, workflowExecutionContext.Bookmarks.ToList());
        }
        
        private WorkflowExecutionContext CreateWorkflowExecutionContext(IServiceProvider serviceProvider, IActivity root, WorkflowState? workflowState = default, Trigger? trigger = default)
        {
            // Build graph.
            var graph = _activityWalker.Walk(root);

            // Assign identities.
            _identityGraphService.AssignIdentities(root);

            // Create scheduler.
            var scheduler = _schedulerFactory.CreateScheduler();

            // Setup a workflow execution context.
            var workflowExecutionContext = new WorkflowExecutionContext(serviceProvider, graph, scheduler, trigger);

            // Restore workflow execution context from state, if provided.
            if (workflowState != null)
            {
                var workflowStateService = serviceProvider.GetRequiredService<IWorkflowStateService>();
                workflowStateService.ApplyState(workflowExecutionContext, workflowState);
            }

            return workflowExecutionContext;
        }
    }
}