using System.Collections.Generic;
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
            IEnumerable<RegisterLocationReference>? locationReferences = default,
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default)
        {
            // Get a handle to the parent execution context.
            var parentActivityExecutionContext = ownerActivity != null ? workflowExecutionContext.ActivityExecutionContexts.First(x => x.Activity == ownerActivity) : default;

            // Setup an activity execution context.
            var register = new Register();
            var expressionExecutionContext = new ExpressionExecutionContext(register, parentActivityExecutionContext?.ExpressionExecutionContext);
            var activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, parentActivityExecutionContext, expressionExecutionContext, new ScheduledActivity(activity, ownerActivity), cancellationToken);

            // Declare locations.
            if (locationReferences != null)
                register.Declare(locationReferences);

            // Push the activity context into the workflow context.
            workflowExecutionContext.ActivityExecutionContexts.Add(activityExecutionContext);

            // Apply execution delegate.
            activityExecutionContext.ExecuteDelegate = executeActivityDelegate;

            // Execute the activity execution pipeline.
            await InvokeAsync(activityExecutionContext);
        }
        
        public async Task InvokeAsync(ActivityExecutionContext activityExecutionContext)
        {
            // Execute the activity execution pipeline.
            await _pipeline.ExecuteAsync(activityExecutionContext);
        }
    }
}