namespace Elsa.Mediator.Contracts;

public interface IRequestHandler<T, in TRequest, TResponse> where TRequest : IRequest<T>
{
    Task<TResponse> HandleAsync(TRequest request);
}