using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.ActivityExecution.Components
{
    public class ActivityDriversMiddleware
    {
        private readonly ActivityMiddlewareDelegate _next;
        private readonly IActivityDriverRegistry _driverRegistry;
        private readonly ILogger _logger;

        public ActivityDriversMiddleware(ActivityMiddlewareDelegate next, IActivityDriverRegistry driverRegistry, ILogger<ActivityDriversMiddleware> logger)
        {
            _next = next;
            _driverRegistry = driverRegistry;
            _logger = logger;
        }

        public async ValueTask InvokeAsync(ActivityExecutionContext context)
        {
            var activity = context.Activity;

            // Get driver.
            var driverActivator = context.WorkflowExecutionContext.GetRequiredService<IActivityDriverActivator>();
            var driver = driverActivator.GetDriver(activity);

            if (driver == null)
            {
                _logger.LogWarning("No driver found for activity {ActivityType}", activity.GetType());
                return;
            }
            
            // Execute driver.
            var methodInfo = typeof(IActivityDriver).GetMethod(nameof(IActivityDriver.ExecuteAsync))!;
            var executeDelegate = context.ExecuteDelegate ?? (ExecuteActivityDelegate)Delegate.CreateDelegate(typeof(ExecuteActivityDelegate), driver, methodInfo);
            await executeDelegate(context);

            // Invoke next middleware.
            await _next(context);

            // Exit if any bookmarks were created.
            if (context.Bookmarks.Any())
                return;

            // Complete parent chain.
            await CompleteParentsAsync(context, activity);
        }

        private static async Task CompleteParentsAsync(ActivityExecutionContext context, IActivity activity)
        {
            var node = context.WorkflowExecutionContext.FindNodeByActivity(activity);
            var currentParent = node.Parent;
            var currentChildContext = context;

            while (currentParent != null)
            {
                var scheduledNodes = context.WorkflowExecutionContext.Scheduler.List().Select(x => context.WorkflowExecutionContext.FindNodeByActivity(x.Activity)).ToList();
                var hasScheduledChildren = scheduledNodes.Any(x => x.Parent?.Activity == activity);

                if (!hasScheduledChildren)
                {
                    // Invoke the completion callback, if any.
                    var parentNode = currentParent;
                    var completionCallback = currentChildContext.WorkflowExecutionContext.PopCompletionCallback(parentNode.Activity);

                    if (completionCallback != null)
                        await completionCallback.Invoke(currentChildContext, parentNode.Activity);
                }

                // Do not continue completion callbacks of parents while there are scheduled nodes.
                if (context.WorkflowExecutionContext.Scheduler.HasAny)
                    break;

                currentChildContext = new ActivityExecutionContext(currentChildContext.WorkflowExecutionContext, new ScheduledActivity(currentParent.Activity), default, currentChildContext.CancellationToken);
                currentParent = currentParent.Parent;
            }
        }
    }

    public static class InvokeDriversMiddlewareExtensions
    {
        public static IActivityExecutionBuilder UseNodeDrivers(this IActivityExecutionBuilder builder) => builder.UseMiddleware<ActivityDriversMiddleware>();
    }
}