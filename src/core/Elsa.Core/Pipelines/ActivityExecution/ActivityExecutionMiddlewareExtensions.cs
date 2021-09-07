using Elsa.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Pipelines.ActivityExecution
{
    public static class ActivityExecutionMiddlewareExtensions
    {
        public static IActivityExecutionBuilder UseMiddleware<TMiddleware>(this IActivityExecutionBuilder builder)
        {
            var middleware = typeof(TMiddleware);

            return builder.Use(next =>
            {
                var invokeMethod = MiddlewareHelpers.GetInvokeMethod(middleware);
                var instance = ActivatorUtilities.CreateInstance(builder.ApplicationServices, middleware, next);
                return (ActivityMiddlewareDelegate)invokeMethod.CreateDelegate(typeof(ActivityMiddlewareDelegate), instance);
            });
        }
    }
}