using Elsa.Mediator.Middleware.Common.Contracts;

namespace Elsa.Mediator.Middleware.Common.Components;

public class CommandLoggingMiddleware : ICommandMiddleware
{
    private readonly CommandMiddlewareDelegate _next;
    public CommandLoggingMiddleware(CommandMiddlewareDelegate next) => _next = next;

    public async ValueTask InvokeAsync(CommandContext context)
    {
        // Invoke next middleware.
        await _next(context);
    }
}