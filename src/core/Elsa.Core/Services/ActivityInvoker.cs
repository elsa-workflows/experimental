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

        public async Task<ActivityExecutionResult> InvokeAsync(WorkflowExecutionContext workflowExecutionContext, IActivity activity, ExecuteActivityDelegate? executeActivityDelegate = default, CancellationToken cancellationToken = default)
        {
            // Setup an activity execution context.
            var activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, new ScheduledActivity(activity), executeActivityDelegate, cancellationToken);

            // Execute the activity execution pipeline.
            await _pipeline.ExecuteAsync(activityExecutionContext);

            return new ActivityExecutionResult();
        }
    }
}