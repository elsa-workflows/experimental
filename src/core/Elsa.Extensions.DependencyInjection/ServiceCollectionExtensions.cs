using System;
using System.Linq;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Options;
using Elsa.Pipelines.ActivityExecution;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Options;
using Elsa.Runtime.Providers;
using Elsa.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElsa(this IServiceCollection services) => services
            .AddElsaCore()
            .AddElsaRuntime()
            .AddDefaultActivities()
            .AddDefaultExpressionHandlers();

        public static IServiceCollection AddElsaCore(this IServiceCollection services)
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
                .AddSingleton<IActivityPortResolver, CodeActivityPortResolver>()
                .AddSingleton<IActivityPortResolver, DynamicActivityPortResolver>()
                .AddLogging();
        }

        private static IServiceCollection AddDefaultActivities(this IServiceCollection services) =>
            services
                .AddExpressionHandler<LiteralHandler>(typeof(Literal<>))
                .AddExpressionHandler<DelegateHandler>(typeof(Delegate<>))
                .AddActivityDriver<SequenceDriver>()
                .AddActivityDriver<WriteLineDriver>()
                .AddActivityDriver<ReadLineDriver>()
                .AddActivityDriver<IfDriver>()
                .AddActivityDriver<ForDriver>()
                .AddActivityDriver<EventDriver>()
                .AddActivityDriver<ForkDriver>();

        private static IServiceCollection AddDefaultExpressionHandlers(this IServiceCollection services) =>
            services
                .AddExpressionHandler<LiteralHandler>(typeof(Literal<>))
                .AddExpressionHandler<DelegateHandler>(typeof(Delegate<>));

        public static IServiceCollection AddElsaRuntime(this IServiceCollection services)
        {
            services.AddOptions<WorkflowRuntimeOptions>();

            return services.AddWorkflowProvider<ConfigurationWorkflowProvider>();
        }

        public static IServiceCollection AddActivityDriver<TDriver, TActivity>(this IServiceCollection services) where TDriver : class, IActivityDriver => services.AddActivityDriver<TDriver>(typeof(TActivity).Name);

        public static IServiceCollection AddActivityDriver<TDriver>(this IServiceCollection services, string? activityTypeName = default) where TDriver : class, IActivityDriver
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

        public static IServiceCollection AddWorkflowProvider<TProvider>(this IServiceCollection services) where TProvider : class, IWorkflowProvider
        {
            services.AddScoped<IWorkflowProvider, TProvider>();

            return services;
        }

        public static IServiceCollection ConfigureWorkflowRuntime(this IServiceCollection services, Action<WorkflowRuntimeOptions> configure) => services.Configure(configure);
    }
}