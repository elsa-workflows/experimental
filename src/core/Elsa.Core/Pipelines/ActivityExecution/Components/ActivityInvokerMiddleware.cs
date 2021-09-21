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
        public static IActivityExecutionBuilder UseActivityDrivers(this IActivityExecutionBuilder builder) => builder.UseMiddleware<ActivityInvokerMiddleware>();
    }
    
    public class ActivityInvokerMiddleware : IActivityExecutionMiddleware
    {
        private readonly ActivityMiddlewareDelegate _next;
        private readonly ILogger _logger;

        public ActivityInvokerMiddleware(ActivityMiddlewareDelegate next, ILogger<ActivityInvokerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async ValueTask InvokeAsync(ActivityExecutionContext context)
        {
            // Evaluate input properties.
            await EvaluateInputPropertiesAsync(context);
            var activity = context.Activity;

            // Execute activity.
            var methodInfo = typeof(IActivity).GetMethod(nameof(IActivity.ExecuteAsync))!;
            var executeDelegate = context.ExecuteDelegate ?? (ExecuteActivityDelegate)Delegate.CreateDelegate(typeof(ExecuteActivityDelegate), activity, methodInfo);
            await executeDelegate(context);

            // Invoke next middleware.
            await _next(context);

            // Exit if any bookmarks were created.
            if (context.Bookmarks.Any())
                return;

            // Complete parent chain.
            await CompleteParentsAsync(context);
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

        private static async Task CompleteParentsAsync(ActivityExecutionContext context)
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
                 
                    // If the activity implements IContainer, notify one of its child activities completed.
                    if (parentNode.Activity is IContainer container && currentChildContext.Node.Parent == currentParent) 
                        await container.CompleteChildAsync(currentChildContext, parentNode.Activity);
                    
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
                
                // Finalize parent.
                CompleteActivity(parentContext);
            }
        }
        
        private static void CompleteActivity(ActivityExecutionContext context) => context.Cleanup();
    }
}