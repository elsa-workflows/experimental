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
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default)
        {
            // Get a reference to the currently executing activity, if any.
            var parentActivityExecutionContext = workflowExecutionContext.CurrentActivityExecutionContext;

            ActivityExecutionContext activityExecutionContext;

            // If the activity to run is the same as the currently executing activity, it means we are resuming.
            if (parentActivityExecutionContext?.Activity == activity)
                activityExecutionContext = parentActivityExecutionContext;
            else
            {
                // Setup an activity execution context.
                var register = new Register();
                var expressionExecutionContext = new ExpressionExecutionContext(register, parentActivityExecutionContext?.ExpressionExecutionContext);
                activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, parentActivityExecutionContext, expressionExecutionContext, new ScheduledActivity(activity), cancellationToken);
                
                // Push the activity context into the workflow context.
                workflowExecutionContext.ActivityExecutionContexts.Push(activityExecutionContext);
            }
            
            // Apply execution delegate.
            activityExecutionContext.ExecuteDelegate = executeActivityDelegate;

            // Execute the activity execution pipeline.
            await _pipeline.ExecuteAsync(activityExecutionContext);
        }
    }
}