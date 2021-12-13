namespace Elsa.Mediator.Contracts;

public interface ICommandHandler
{
}

public interface ICommandHandler<in TCommand, TResult> : ICommandHandler where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand
{
}