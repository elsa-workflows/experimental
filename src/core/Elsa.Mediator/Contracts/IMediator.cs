namespace Elsa.Mediator.Contracts;

public interface IMediator
{
    Task<T> RequestAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default);
    Task<T> ExecuteAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default);
    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);
}