using Elsa.Mediator.Contracts;

namespace Elsa.Mediator.Services;

public class DefaultMediator : IMediator
{
    private readonly IEnumerable<ICommandHandler> _commandHandlers;
    private readonly IEnumerable<IRequestHandler> _requestHandlers;
    private readonly IEnumerable<INotificationHandler> _notificationHandlers;

    public DefaultMediator(
        IEnumerable<ICommandHandler> commandHandlers,
        IEnumerable<IRequestHandler> requestHandlers,
        IEnumerable<INotificationHandler> notificationHandlers)
    {
        _commandHandlers = commandHandlers;
        _requestHandlers = requestHandlers;
        _notificationHandlers = notificationHandlers;
    }

    public async Task<T> RequestAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default)
    {
        // Find all handlers for the specified request.
        var requestType = request.GetType();
        var resultType = typeof(T);
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, resultType);
        var handlers = _requestHandlers.Where(x => handlerType.IsInstanceOfType(x)).ToArray();

        if (!handlers.Any())
            throw new InvalidOperationException($"There is no handler to handle the {requestType.FullName} command");

        if (handlers.Length > 1)
            throw new InvalidOperationException($"Multiple handlers were found to handle the {requestType.FullName} command");

        var handler = handlers.First();
        var handleMethod = handlerType.GetMethod("HandleAsync")!;
        var task = (Task<T>)handleMethod.Invoke(handler, new object?[] { request, cancellationToken })!;

        return await task;
    }

    public async Task<T> ExecuteAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default)
    {
        // Find all handlers for the specified command.
        var commandType = command.GetType();
        var resultType = typeof(T);
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);
        var handlers = _commandHandlers.Where(x => handlerType.IsInstanceOfType(x)).ToArray();

        if (!handlers.Any())
            throw new InvalidOperationException($"There is no handler to handle the {commandType.FullName} command");

        if (handlers.Length > 1)
            throw new InvalidOperationException($"Multiple handlers were found to handle the {commandType.FullName} command");

        var handler = handlers.First();
        var handleMethod = handlerType.GetMethod("HandleAsync")!;
        var task = (Task<T>)handleMethod.Invoke(handler, new object?[] { command, cancellationToken })!;

        return await task;
    }

    public async Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        // Find all handlers for the specified notification.
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var handlers = _notificationHandlers.Where(x => handlerType.IsInstanceOfType(x)).ToArray();
        var handleMethod = handlerType.GetMethod("HandleAsync")!;

        foreach (var handler in handlers)
        {
            var task = (Task)handleMethod.Invoke(handler, new object?[] { notification, cancellationToken })!;
            await task;
        }
    }
}