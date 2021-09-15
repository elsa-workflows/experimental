using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;
using Microsoft.Extensions.Logging;
using Delegate = System.Delegate;

namespace Elsa.Pipelines.ActivityExecution.Components
{
    public static class InvokeDriversMiddlewareExtensions
    {
        public static IActivityExecutionBuilder UseActivityDrivers(this IActivityExecutionBuilder builder) => builder.UseMiddleware<ActivityDriversMiddleware>();
    }
    
    public class ActivityDriversMiddleware : IActivityExecutionMiddleware
    {
        private readonly ActivityMiddlewareDelegate _next;
        private readonly ILogger _logger;

        public ActivityDriversMiddleware(ActivityMiddlewareDelegate next, ILogger<ActivityDriversMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async ValueTask InvokeAsync(ActivityExecutionContext context)
        {
            // Evaluate input properties.
            await EvaluateInputPropertiesAsync(context);

            // Get driver.
            var activity = context.Activity;
            var driverActivator = context.WorkflowExecutionContext.GetRequiredService<IActivityDriverActivator>();
            var driver = driverActivator.ActivateDriver(activity);

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
            await CompleteParentsAsync(context, driverActivator);
        }

        private async Task EvaluateInputPropertiesAsync(ActivityExecutionContext context)
        {
            var activity = context.Activity;
            var inputs = activity.GetInputs();
            var assignedInputs = inputs.Where(x => x.LocationReference != null!).ToList();
            var evaluator = context.WorkflowExecutionContext.GetRequiredService<IExpressionEvaluator>();

            foreach (var input in assignedInputs)
            {
                var locationReference = input.LocationReference;
                var value = await evaluator.EvaluateAsync(input.Expression, new ExpressionExecutionContext(context));
                locationReference.Set(context, value);
            }
        }

        private static async Task CompleteParentsAsync(ActivityExecutionContext context, IActivityDriverActivator driverActivator)
        {
            var activity = context.Activity;
            var node = context.WorkflowExecutionContext.FindNodeByActivity(activity);
            var currentParent = node.Parent;
            var currentChildContext = context;

            while (currentParent != null)
            {
                var scheduledNodes = context.WorkflowExecutionContext.Scheduler.List().Select(x => context.WorkflowExecutionContext.FindNodeByActivity(x.Activity)).ToList();
                var hasScheduledChildren = scheduledNodes.Any(x => x.Parent?.Activity == activity);
                var parentContext = currentChildContext.ParentActivityExecutionContext!;
                
                if (!hasScheduledChildren)
                {
                    // Notify the parent activity about the child's completion.
                    var parentNode = currentParent;
                 
                    // If the driver implements IContainerDriver, invoke it.
                    if (driverActivator.ActivateDriver(parentNode.Activity) is IContainerDriver containerDriver) 
                        await containerDriver.OnChildCompleteAsync(currentChildContext, parentNode.Activity);
                    
                    // Invoke any completion callback.
                    var completionCallback = currentChildContext.WorkflowExecutionContext.PopCompletionCallback(parentNode.Activity);

                    if (completionCallback != null) 
                        await completionCallback.Invoke(currentChildContext, parentContext);

                    // Handle activity completion.
                    CompleteActivity(currentChildContext);
                }

                // Do not continue completion callbacks of parents while there are scheduled nodes.
                if (context.WorkflowExecutionContext.Scheduler.HasAny)
                    break;

                currentChildContext = parentContext;
                currentParent = currentParent.Parent;
            }
        }
        
        private static void CompleteActivity(ActivityExecutionContext context) => context.Cleanup();
    }
}