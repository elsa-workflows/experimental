using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Elsa.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Pipelines.NodeExecution
{
    public static class NodeExecutionMiddlewareExtensions
    {
        private const string InvokeMethodName = "Invoke";
        private const string InvokeAsyncMethodName = "InvokeAsync";

        public static INodeExecutionBuilder UseMiddleware<TMiddleware>(this INodeExecutionBuilder builder)
        {
            var middleware = typeof(TMiddleware);

            return builder.Use(next =>
            {
                var invokeMethod = GetInvokeMethod(middleware);
                var instance = ActivatorUtilities.CreateInstance(builder.ApplicationServices, middleware, next);
                return (ExecuteNodeMiddlewareDelegate)invokeMethod.CreateDelegate(typeof(ExecuteNodeMiddlewareDelegate), instance);
            });
        }

        private static MethodInfo GetInvokeMethod(Type middleware)
        {
            var methods = middleware.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var invokeMethods = methods.Where(m => string.Equals(m.Name, InvokeMethodName, StringComparison.Ordinal) || string.Equals(m.Name, InvokeAsyncMethodName, StringComparison.Ordinal)).ToArray();

            switch (invokeMethods.Length)
            {
                case > 1:
                    throw new InvalidOperationException("Multiple Invoke methods were found. Use either Invoke or InvokeAsync.");
                case 0:
                    throw new InvalidOperationException("No Invoke methods were found. Use either Invoke or InvokeAsync");
            }

            var methodInfo = invokeMethods[0];

            if (!typeof(Task).IsAssignableFrom(methodInfo.ReturnType) && !typeof(ValueTask).IsAssignableFrom(methodInfo.ReturnType))
                throw new InvalidOperationException($"The {methodInfo.Name} method must return Task or ValueTask");

            return methodInfo;
        }
    }
}