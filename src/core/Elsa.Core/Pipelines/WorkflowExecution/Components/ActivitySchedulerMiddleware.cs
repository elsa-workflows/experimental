using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.WorkflowExecution.Components
{
    public static class UseActivitySchedulerMiddlewareExtensions
    {
        public static IWorkflowExecutionBuilder UseActivityScheduler(this IWorkflowExecutionBuilder builder) => builder.UseMiddleware<ActivitySchedulerMiddleware>();
    }
    
    public class ActivitySchedulerMiddleware : IWorkflowExecutionMiddleware
    {
        private readonly WorkflowMiddlewareDelegate _next;
        private readonly ILogger _logger;

        public ActivitySchedulerMiddleware(WorkflowMiddlewareDelegate next, ILogger<ActivitySchedulerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async ValueTask InvokeAsync(WorkflowExecutionContext context)
        {
            var scheduler = context.Scheduler;
            var executeActivityDelegate = context.ExecuteDelegate;
            var cancellationToken = context.CancellationToken;
            var activityInvoker = context.GetRequiredService<IActivityInvoker>();

            // As long as there are activities scheduled, keep executing them.
            while (scheduler.HasAny)
            {
                // Pop next activity for execution.
                var currentActivity = scheduler.Pop();

                // Execute activity.
                await activityInvoker.InvokeAsync(context, currentActivity.Activity, executeActivityDelegate, cancellationToken);
                
                // Reset custom activity execution delegate. This is used only once for the initial activity being executed.
                executeActivityDelegate = null;
            }

            // Invoke next middleware.
            await _next(context);
        }
    }
}