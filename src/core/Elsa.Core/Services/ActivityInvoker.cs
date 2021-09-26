using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityInvoker : IActivityInvoker
    {
        private readonly IActivityExecutionPipeline _pipeline;

        public ActivityInvoker(IActivityExecutionPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public async Task InvokeAsync(
            WorkflowExecutionContext workflowExecutionContext,
            IActivity activity,
            IActivity? ownerActivity,
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default)
        {
            // Get a reference to the currently executing activity, if any.
            var activityExecutionContext = workflowExecutionContext.ActivityExecutionContexts.FirstOrDefault(x => x.Activity == activity);
            var parentActivityExecutionContext = ownerActivity != null ? workflowExecutionContext.ActivityExecutionContexts.First(x => x.Activity == ownerActivity) : activityExecutionContext?.ParentActivityExecutionContext;

            if (activityExecutionContext == null)
            {
                // Setup an activity execution context.
                var register = new Register();
                var expressionExecutionContext = new ExpressionExecutionContext(register, parentActivityExecutionContext?.ExpressionExecutionContext);
                activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, parentActivityExecutionContext, expressionExecutionContext, new ScheduledActivity(activity, ownerActivity), cancellationToken);

                // Push the activity context into the workflow context.
                workflowExecutionContext.ActivityExecutionContexts.Add(activityExecutionContext);
            }

            // Apply execution delegate.
            activityExecutionContext.ExecuteDelegate = executeActivityDelegate;

            // Execute the activity execution pipeline.
            await _pipeline.ExecuteAsync(activityExecutionContext);
        }
    }
}