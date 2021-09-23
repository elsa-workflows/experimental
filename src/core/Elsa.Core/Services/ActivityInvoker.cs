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
            // Set current activity.
            workflowExecutionContext.CurrentActivity = activity;
            
            // Setup an expression execution context.
            var register = workflowExecutionContext.GetOrCreateRegister(activity);
            var expressionExecutionContext = new ExpressionExecutionContext(register);
         
            // Check if there is already an activity execution context for the activity.
            var activityExecutionContext = workflowExecutionContext.ActivityExecutionContexts.FirstOrDefault(x => x.Activity == activity);

            if (activityExecutionContext == null)
            {
                // Setup an activity execution context.
                activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, expressionExecutionContext, new ScheduledActivity(activity), cancellationToken);
                
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