namespace Elsa.Mediator.Middleware.Common.Contracts;

public interface ICommandMiddleware
{
    ValueTask InvokeAsync(CommandContext context);
}