using Elsa.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Pipelines.WorkflowExecution
{
    public static class WorkflowExecutionMiddlewareExtensions
    {
        public static IWorkflowExecutionBuilder UseMiddleware<TMiddleware>(this IWorkflowExecutionBuilder builder)
        {
            var middleware = typeof(TMiddleware);

            return builder.Use(next =>
            {
                var invokeMethod = MiddlewareHelpers.GetInvokeMethod(middleware);
                var instance = ActivatorUtilities.CreateInstance(builder.ApplicationServices, middleware, next);
                return (WorkflowMiddlewareDelegate)invokeMethod.CreateDelegate(typeof(WorkflowMiddlewareDelegate), instance);
            });
        }
    }
}