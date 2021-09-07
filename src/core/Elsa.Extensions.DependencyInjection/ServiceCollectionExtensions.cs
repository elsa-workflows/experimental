using System;
using System.Linq;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Options;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Pipelines.ActivityExecution;
using Elsa.Pipelines.WorkflowExecution;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.HostedServices;
using Elsa.Runtime.Instructions;
using Elsa.Runtime.Options;
using Elsa.Runtime.Services;
using Elsa.Runtime.Stimuli.Handlers;
using Elsa.Runtime.WorkflowProviders;
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

                // Core.
                .AddSingleton<IActivityInvoker, ActivityInvoker>()
                .AddSingleton<IWorkflowInvoker, WorkflowInvoker>()
                .AddSingleton<IActivityDriverRegistry, ActivityDriverRegistry>()
                .AddSingleton<IActivityWalker, ActivityWalker>()
                .AddSingleton<IIdentityGraphService, IdentityGraphService>()
                .AddSingleton<IWorkflowStateService, WorkflowStateService>()
                .AddSingleton<IActivitySchedulerFactory, ActivitySchedulerFactory>()
                .AddSingleton<IActivityPortResolver, CodeActivityPortResolver>()
                .AddSingleton<IActivityPortResolver, DynamicActivityPortResolver>()
                .AddSingleton<IHasher, Hasher>()
                .AddScoped<IActivityDriverActivator, ActivityDriverActivator>()

                // Expressions.
                .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
                .AddSingleton<IExpressionHandlerRegistry, ExpressionHandlerRegistry>()

                // Pipelines.
                .AddSingleton<IActivityExecutionPipeline, ActivityExecutionPipeline>()
                .AddSingleton<IWorkflowExecutionPipeline, WorkflowExecutionPipeline>()

                // Logging
                .AddLogging();
        }

        public static IServiceCollection AddElsaRuntime(this IServiceCollection services)
        {
            services.AddOptions<WorkflowRuntimeOptions>();

            return services
                // Core.
                .AddSingleton<IWorkflowRegistry, WorkflowRegistry>()
                .AddSingleton<IWorkflowInstructionManager, WorkflowInstructionManager>()
                .AddSingleton<ITriggerIndexer, TriggerIndexer>()
                
                // Stimulus handlers.
                .AddStimulusHandler<TriggerWorkflowsStimulusHandler>()
                .AddStimulusHandler<ResumeWorkflowsStimulusHandler>()
                
                // Instruction handlers.
                .AddInstructionHandler<TriggerWorkflowInstructionHandler>()
                .AddInstructionHandler<ResumeWorkflowInstructionHandler>()

                // Workflow providers.
                .AddWorkflowProvider<ConfigurationWorkflowProvider>()
;
        }

        private static IServiceCollection AddDefaultActivities(this IServiceCollection services) =>
            services
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

        public static IServiceCollection AddTriggerProvider<T>(this IServiceCollection services) where T : class, ITriggerProvider => services.AddSingleton<ITriggerProvider, T>();
        public static IServiceCollection AddWorkflowProvider<T>(this IServiceCollection services) where T : class, IWorkflowProvider => services.AddSingleton<IWorkflowProvider, T>();
        public static IServiceCollection AddStimulusHandler<T>(this IServiceCollection services) where T : class, IStimulusHandler => services.AddSingleton<IStimulusHandler, T>();
        public static IServiceCollection AddInstructionHandler<T>(this IServiceCollection services) where T : class, IWorkflowInstructionHandler => services.AddSingleton<IWorkflowInstructionHandler, T>();
        public static IServiceCollection ConfigureWorkflowRuntime(this IServiceCollection services, Action<WorkflowRuntimeOptions> configure) => services.Configure(configure);
        public static IServiceCollection IndexWorkflowTriggers(this IServiceCollection services) => services.AddHostedService<IndexWorkflowTriggers>();
        public static IServiceCollection AddWorkflowInstanceStore<T>(this IServiceCollection services) where T : class, IWorkflowInstanceStore => services.AddSingleton<IWorkflowInstanceStore, T>();
        public static IServiceCollection AddBookmarkStore<T>(this IServiceCollection services) where T : class, IWorkflowBookmarkStore => services.AddSingleton<IWorkflowBookmarkStore, T>();
        public static IServiceCollection AddTriggerStore<T>(this IServiceCollection services) where T : class, IWorkflowTriggerStore => services.AddSingleton<IWorkflowTriggerStore, T>();
        public static IServiceCollection AddWorkflowInstanceStore(this IServiceCollection services, IWorkflowInstanceStore store) => services.AddSingleton(store);
        public static IServiceCollection AddBookmarkStore(this IServiceCollection services, IWorkflowBookmarkStore store) => services.AddSingleton(store);
        public static IServiceCollection AddTriggerStore(this IServiceCollection services, IWorkflowTriggerStore store) => services.AddSingleton(store);
    }
}