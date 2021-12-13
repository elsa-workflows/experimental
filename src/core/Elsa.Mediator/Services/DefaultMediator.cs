using Elsa.Mediator.Contracts;

namespace Elsa.Mediator.Services;

public class DefaultMediator : IMediator
{
    private readonly IEnumerable<ICommandHandler> _commandHandlers;

    public DefaultMediator(IEnumerable<ICommandHandler> commandHandlers)
    {
        _commandHandlers = commandHandlers;
    }
    
    public Task<T> SendRequestAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<T> SendCommandAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default)
    {
        // Find all handlers for the specified command.
        var commandType = command.GetType();
        var resultType = typeof(T);
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType, resultType);
        var handler = _commandHandlers.FirstOrDefault(x => x.GetType() == handlerType);
        var handleMethod = handlerType.GetMethod("HandleAsync")!;
        
        handleMethod.Invoke(handler, new object?[] { command });
    }
}