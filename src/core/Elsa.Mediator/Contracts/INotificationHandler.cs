namespace Elsa.Mediator.Contracts;

public interface INotificationHandler<in T> where T : INotification
{
    Task HandleAsync(T notification);
}