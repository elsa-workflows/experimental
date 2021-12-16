using Elsa.Mediator.Middleware.Common.Components;
using Elsa.Mediator.Middleware.Common.Contracts;

namespace Elsa.Mediator.Middleware.Common;

public static class CommandPipelineBuilderExtensions
{
    public static ICommandPipelineBuilder UseCommandHandlers(this ICommandPipelineBuilder builder) => builder.UseMiddleware<CommandHandlerInvokerMiddleware>();
    public static ICommandPipelineBuilder UseCommandLogging(this ICommandPipelineBuilder builder) => builder.UseMiddleware<CommandLoggingMiddleware>();
}