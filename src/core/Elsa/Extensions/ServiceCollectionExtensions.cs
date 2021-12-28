using System;
using Elsa.ActivityNodeResolvers;
using Elsa.Contracts;
using Elsa.Dsl.Abstractions;
using Elsa.Dsl.Extensions;
using Elsa.Dsl.Services;
using Elsa.Expressions;
using Elsa.Extensions;
using Elsa.Options;
using Elsa.Pipelines.ActivityExecution;
using Elsa.Pipelines.WorkflowExecution;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.HostedServices;
using Elsa.Runtime.Interpreters;
using Elsa.Runtime.Options;
using Elsa.Runtime.Services;
using Elsa.Runtime.Stimuli.Handlers;
using Elsa.Runtime.WorkflowProviders;
using Elsa.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElsa(this IServiceCollection services) => services
        .AddElsaCore()
        .AddElsaRuntime()
        .AddDefaultExpressionHandlers();

    public static IServiceCollection AddElsaCore(this IServiceCollection services)
    {
        services.AddOptions<WorkflowEngineOptions>();

        return services

            // Core.
            .AddSingleton<IActivityInvoker, ActivityInvoker>()
            .AddSingleton<IWorkflowEngine, WorkflowEngine>()
            .AddSingleton<IActivityWalker, ActivityWalker>()
            .AddSingleton<IIdentityGraphService, IdentityGraphService>()
            .AddSingleton<IWorkflowStateSerializer, WorkflowStateSerializer>()
            .AddSingleton<IActivitySchedulerFactory, ActivitySchedulerFactory>()
            .AddSingleton<IActivityNodeResolver, OutboundActivityNodeResolver>()
            .AddSingleton<ITypeSystem, TypeSystem>()
            .AddSingleton<IHasher, Hasher>()
            .AddSingleton<ISystemClock, SystemClock>()

            // DSL.
            .AddDsl()

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
                .AddSingleton<IStimulusInterpreter, StimulusInterpreter>()
                .AddSingleton<IWorkflowInstructionExecutor, WorkflowInstructionExecutor>()
                .AddSingleton<ITriggerIndexer, TriggerIndexer>()

                // Stimulus handlers.
                .AddStimulusHandler<TriggerWorkflowsStimulusHandler>()
                .AddStimulusHandler<ResumeWorkflowsStimulusHandler>()

                // Instruction interpreters.
                .AddInstructionInterpreter<TriggerWorkflowInstructionInterpreter>()
                .AddInstructionInterpreter<ResumeWorkflowInstructionInterpreter>()

                // Workflow providers.
                .AddWorkflowProvider<ConfigurationWorkflowProvider>()
                .AddWorkflowProvider<DatabaseWorkflowProvider>()
                
                // Workflow engine.
                .AddSingleton<IWorkflowServer, WorkflowServer>()
            ;
    }

    private static IServiceCollection AddDefaultExpressionHandlers(this IServiceCollection services) =>
        services
            .AddExpressionHandler<LiteralHandler, LiteralExpression>()
            .AddExpressionHandler<DelegateExpressionHandler, DelegateExpression>()
            .AddExpressionHandler<VariableExpressionHandler, VariableExpression>()
            .AddExpressionHandler<ElsaExpressionHandler, ElsaExpression>();

    public static IServiceCollection AddWorkflowProvider<T>(this IServiceCollection services) where T : class, IWorkflowProvider => services.AddSingleton<IWorkflowProvider, T>();
    public static IServiceCollection AddStimulusHandler<T>(this IServiceCollection services) where T : class, IStimulusHandler => services.AddSingleton<IStimulusHandler, T>();
    public static IServiceCollection AddInstructionInterpreter<T>(this IServiceCollection services) where T : class, IWorkflowInstructionInterpreter => services.AddSingleton<IWorkflowInstructionInterpreter, T>();
    public static IServiceCollection ConfigureWorkflowRuntime(this IServiceCollection services, Action<WorkflowRuntimeOptions> configure) => services.Configure(configure);
    public static IServiceCollection IndexWorkflowTriggers(this IServiceCollection services) => services.AddHostedService<IndexWorkflowTriggers>();
}