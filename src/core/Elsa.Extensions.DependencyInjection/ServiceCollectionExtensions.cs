using System;
using System.Linq;
using Elsa.Contracts;
using Elsa.Options;
using Elsa.Pipelines.NodeExecution;
using Elsa.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElsa(this IServiceCollection services)
        {
            services.AddOptions<WorkflowEngineOptions>();

            return services
                .AddSingleton<INodeInvoker, NodeInvoker>()
                .AddSingleton<INodeDriverRegistry, NodeDriverRegistry>()
                .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
                .AddSingleton<IExpressionHandlerRegistry, ExpressionHandlerRegistry>()
                .AddSingleton<INodeExecutionPipeline, NodeExecutionPipeline>()
                .AddSingleton<INodeWalker, NodeWalker>()
                .AddSingleton<IIdentityGraphService, IdentityGraphService>()
                .AddSingleton<IWorkflowStateService, WorkflowStateService>()
                .AddSingleton<INodeSchedulerFactory, NodeSchedulerFactory>()
                .AddLogging();
        }

        public static IServiceCollection AddNodeDriver<TDriver>(this IServiceCollection services) where TDriver : class, INodeDriver
        {
            var driverType = typeof(TDriver);
            var nodeType = driverType.BaseType!.GetGenericArguments().FirstOrDefault();

            if (nodeType == null)
                if (typeof(INode).IsAssignableFrom(driverType))
                    nodeType = typeof(TDriver);
                else
                    throw new InvalidOperationException();

            // Register driver with DI.
            services.AddScoped<TDriver>();

            // Register driver with options.
            services.Configure<WorkflowEngineOptions>(elsa => elsa.RegisterNodeDriver(nodeType, driverType));

            return services;
        }

        public static IServiceCollection AddExpressionHandler<THandler>(this IServiceCollection services, Type expression) where THandler : class, IExpressionHandler
        {
            // Register handler with DI.
            services.AddSingleton<THandler>();

            // Register handler with options.
            services.Configure<WorkflowEngineOptions>(elsa => elsa.RegisterExpressionHandler<THandler>(expression));

            return services;
        }
    }
}