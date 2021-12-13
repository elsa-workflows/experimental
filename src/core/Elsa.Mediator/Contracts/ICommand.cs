namespace Elsa.Mediator.Contracts;

public interface ICommand<T>
{
}

public interface ICommand : ICommand<Unit>
{
}