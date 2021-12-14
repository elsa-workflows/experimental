using Elsa.Mediator.Contracts;
using Elsa.Mediator.Extensions;
using Elsa.Persistence.Abstractions.Commands;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.InMemory.CommandHandlers;
using Elsa.Persistence.InMemory.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Persistence.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryWorkflowInstanceStore(this IServiceCollection services) => services.AddSingleton<IWorkflowInstanceStore, InMemoryWorkflowInstanceStore>();
    public static IServiceCollection AddInMemoryBookmarkStore(this IServiceCollection services) => services.AddSingleton<IWorkflowBookmarkStore, InMemoryWorkflowBookmarkStore>();
    public static IServiceCollection AddInMemoryTriggerStore(this IServiceCollection services) => services.AddSingleton<IWorkflowTriggerStore, InMemoryWorkflowTriggerStore>();

    public static IServiceCollection AddInMemoryCommands(this IServiceCollection services)
    {
        return services.AddCommandHandler<SaveWorkflowDefinitionHandler, SaveWorkflowDefinition>();
    }
}