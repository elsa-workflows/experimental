using Elsa.Mediator.Contracts;
using Elsa.Mediator.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Mediator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        return services.AddSingleton<IMediator, DefaultMediator>();
    }

    public static IServiceCollection AddCommandHandler<THandler, TCommand>(this IServiceCollection services)
        where THandler : class, ICommandHandler<TCommand>
        where TCommand : ICommand =>
        services.AddCommandHandler<THandler, TCommand, Unit>();

    public static IServiceCollection AddCommandHandler<THandler, TCommand, TResult>(this IServiceCollection services)
        where THandler : class, ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        return services.AddSingleton<ICommandHandler, THandler>();
    }
}