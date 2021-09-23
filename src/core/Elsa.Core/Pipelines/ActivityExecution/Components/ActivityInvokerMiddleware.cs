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
            {
                // Store bookmarks.
                context.WorkflowExecutionContext.RegisterBookmarks(context.Bookmarks);
                
                // Block current path of execution.
                return;
            }
            
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
                var value = await evaluator.EvaluateAsync(input.Expression, context.ExpressionExecutionContext);
                locationReference.Set(context, value);
            }
        }

        private static async Task CompleteParentsAsync(ActivityExecutionContext context)
        {
            var workflowExecutionContext = context.WorkflowExecutionContext;
            var activity = context.Activity;
            var node = context.WorkflowExecutionContext.FindNodeByActivity(activity);
            var currentParent = node.Parent;
            var currentContext = context;

            while (currentParent != null)
            {
                var parentActivity = currentParent.Activity;
                var scheduledNodes = workflowExecutionContext.Scheduler.List().Select(x => workflowExecutionContext.FindNodeByActivity(x.Activity)).ToList();
                var hasScheduledChildren = scheduledNodes.Any(x => x.Parent?.Activity == activity);

                // Establish context for parent.
                var parentContext = workflowExecutionContext.ActivityExecutionContexts.FirstOrDefault(x => x.Activity == parentActivity);

                if (parentContext == null)
                {
                    var register = workflowExecutionContext.GetOrCreateRegister(activity);
                    var expressionExecutionContext = new ExpressionExecutionContext(register);
                    parentContext = new ActivityExecutionContext(workflowExecutionContext, expressionExecutionContext, currentContext.ScheduledActivity, workflowExecutionContext.CancellationToken);
                }
                
                if (!hasScheduledChildren)
                {
                    // If the activity implements IContainer, notify that one of its child activities completed.
                    if (parentActivity is IContainer container)
                    {
                        await container.CompleteChildAsync(parentContext, currentContext);
                    }

                    // Invoke any completion callback.
                    var completionCallback = workflowExecutionContext.PopCompletionCallback(currentParent.Activity);

                    if (completionCallback != null)
                        await completionCallback.Invoke(currentContext, currentContext);

                }

                // Do not continue completion callbacks of parents while there are scheduled nodes.
                if (workflowExecutionContext.Scheduler.HasAny)
                    break;

                currentParent = currentParent.Parent;
                currentContext = parentContext;
            }
        }

        private static void CompleteActivity(ActivityExecutionContext context) => context.Cleanup();
    }
}