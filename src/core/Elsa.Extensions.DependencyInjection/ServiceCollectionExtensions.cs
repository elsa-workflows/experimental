using System;
using System.Linq;
using Elsa.Contracts;
using Elsa.Options;
using Elsa.Pipelines.ActivityExecution;
using Elsa.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElsa(this IServiceCollection services)
        {
            services.AddOptions<WorkflowEngineOptions>();

            return services
                .AddSingleton<IActivityInvoker, ActivityInvoker>()
                .AddSingleton<IActivityDriverRegistry, ActivityDriverRegistry>()
                .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
                .AddSingleton<IExpressionHandlerRegistry, ExpressionHandlerRegistry>()
                .AddSingleton<INodeExecutionPipeline, NodeExecutionPipeline>()
                .AddSingleton<IActivityWalker, ActivityWalker>()
                .AddSingleton<IIdentityGraphService, IdentityGraphService>()
                .AddSingleton<IWorkflowStateService, WorkflowStateService>()
                .AddSingleton<IActivitySchedulerFactory, ActivitySchedulerFactory>()
                .AddLogging();
        }

        public static IServiceCollection AddNodeDriver<TDriver, TActivity>(this IServiceCollection services) where TDriver : class, IActivityDriver => services.AddNodeDriver<TDriver>(typeof(TActivity).Name);

        public static IServiceCollection AddNodeDriver<TDriver>(this IServiceCollection services, string? activityTypeName = default) where TDriver : class, IActivityDriver
        {
            var driverType = typeof(TDriver);
            var activityType = driverType.BaseType!.GetGenericArguments().FirstOrDefault();
            activityTypeName ??= activityType?.Name;

            if (activityTypeName == null)
                // Check if the activity type implements the driver.
                if (typeof(IActivity).IsAssignableFrom(driverType))
                    activityTypeName = typeof(TDriver).Name;
                else
                    throw new Exception("Failed to determine the activity type name. Please provide the activity type name explicitly via the activityTypeName parameter");

            // Register driver with DI.
            services.AddScoped<TDriver>();

            // Register driver with options.
            services.Configure<WorkflowEngineOptions>(elsa => elsa.RegisterNodeDriver(activityTypeName, driverType));

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