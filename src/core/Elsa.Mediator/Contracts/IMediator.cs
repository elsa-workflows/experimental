namespace Elsa.Mediator.Contracts;

public interface IMediator
{
    Task<T> SendRequestAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default);
    Task<T> SendCommandAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default);
}