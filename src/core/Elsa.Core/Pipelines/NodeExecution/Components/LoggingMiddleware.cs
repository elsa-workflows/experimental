using System.Diagnostics;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.NodeExecution.Components
{
    public class LoggingMiddleware
    {
        private readonly ExecuteNode _next;
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch;

        public LoggingMiddleware(ExecuteNode next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async ValueTask InvokeAsync(NodeExecutionContext context)
        {
            var node = context.Node;
            _logger.LogDebug("Executing node {Node}", node.GetType().Name);
            _stopwatch.Restart();
            await _next(context);
            _stopwatch.Stop();
            _logger.LogDebug("Executed node {Node} in {Elapsed}", node.GetType().Name, _stopwatch.Elapsed);
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseLogging(this INodeExecutionBuilder builder) => builder.UseMiddleware<LoggingMiddleware>();
    }
}