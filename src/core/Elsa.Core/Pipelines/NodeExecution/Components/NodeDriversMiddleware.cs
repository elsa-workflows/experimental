using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.NodeExecution.Components
{
    public class NodeDriversMiddleware
    {
        private readonly ExecuteNode _next;
        private readonly INodeDriverRegistry _driverRegistry;
        private readonly ILogger<NodeDriversMiddleware> _logger;

        public NodeDriversMiddleware(ExecuteNode next, INodeDriverRegistry driverRegistry, ILogger<NodeDriversMiddleware> logger)
        {
            _next = next;
            _driverRegistry = driverRegistry;
            _logger = logger;
        }

        public async ValueTask InvokeAsync(NodeExecutionContext context)
        {
            var node = context.Node;
            var driver = _driverRegistry.GetDriver(node);

            if (driver == null)
            {
                _logger.LogWarning("No driver found for node {NodeType}", node.GetType());
                return;
            }
                
            var result = await driver.ExecuteAsync(context);
            await result.ExecuteAsync(context);
            await _next(context);
        }
    }

    public static class ExecuteNodeMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseNodeDrivers(this INodeExecutionBuilder builder) => builder.UseMiddleware<NodeDriversMiddleware>();
    }
}