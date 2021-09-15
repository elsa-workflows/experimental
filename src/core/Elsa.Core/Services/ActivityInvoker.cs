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
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default)
        {
            // Get a reference to the current activity.
            var currentActivityExecutionContext = workflowExecutionContext.ActivityExecutionContexts.Any() ? workflowExecutionContext.ActivityExecutionContexts.Peek() : default;
            
            // Set current activity.
            workflowExecutionContext.CurrentActivity = activity;
            
            // Setup an activity execution context.
            var activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, currentActivityExecutionContext, new ScheduledActivity(activity), executeActivityDelegate, cancellationToken);
            
            // Push the activity context into the workflow context.
            workflowExecutionContext.ActivityExecutionContexts.Push(activityExecutionContext);

            // Execute the activity execution pipeline.
            await _pipeline.ExecuteAsync(activityExecutionContext);
        }
    }
}