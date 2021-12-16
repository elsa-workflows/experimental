namespace Elsa.Mediator.Middleware.Common.Contracts;

public interface ICommandPipeline
{
    CommandMiddlewareDelegate Setup(Action<ICommandPipelineBuilder> setup);
    CommandMiddlewareDelegate Pipeline { get; }
    Task ExecuteAsync(CommandContext context);
}