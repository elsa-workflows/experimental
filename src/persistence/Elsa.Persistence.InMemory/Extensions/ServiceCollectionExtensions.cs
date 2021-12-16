using Elsa.Mediator.Extensions;
using Elsa.Persistence.Entities;
using Elsa.Persistence.InMemory.Handlers.Commands;
using Elsa.Persistence.InMemory.Services;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS8631

namespace Elsa.Persistence.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
    {
        return services
                .AddInMemoryHandlers()
                //.AddInMemoryWorkflowInstanceStore()
                //.AddInMemoryBookmarkStore()
                //.AddInMemoryTriggerStore()
                .AddSingleton<InMemoryStore<WorkflowDefinition>>()
                .AddSingleton<InMemoryStore<WorkflowInstance>>()
                .AddSingleton<InMemoryStore<WorkflowBookmark>>()
                .AddSingleton<InMemoryStore<WorkflowTrigger>>()
            ;
    }

    //public static IServiceCollection AddInMemoryWorkflowInstanceStore(this IServiceCollection services) => services.AddSingleton<IWorkflowInstanceStore, InMemoryWorkflowInstanceStore>();
    //public static IServiceCollection AddInMemoryBookmarkStore(this IServiceCollection services) => services.AddSingleton<IWorkflowBookmarkStore, InMemoryWorkflowBookmarkStore>();
    //public static IServiceCollection AddInMemoryTriggerStore(this IServiceCollection services) => services.AddSingleton<IWorkflowTriggerStore, InMemoryWorkflowTriggerStore>();
    public static IServiceCollection AddInMemoryHandlers(this IServiceCollection services) => services.AddHandlersFrom<SaveWorkflowDefinitionHandler>();
}