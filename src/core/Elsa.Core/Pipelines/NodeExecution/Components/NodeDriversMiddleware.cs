using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.NodeExecution.Components
{
    public class NodeDriversMiddleware
    {
        private readonly ExecuteNodeDelegate _next;
        private readonly INodeDriverRegistry _driverRegistry;
        private readonly ILogger _logger;

        public NodeDriversMiddleware(ExecuteNodeDelegate next, INodeDriverRegistry driverRegistry, ILogger<NodeDriversMiddleware> logger)
        {
            _next = next;
            _driverRegistry = driverRegistry;
            _logger = logger;
        }

        public async ValueTask InvokeAsync(NodeExecutionContext context)
        {
            var node = context.Node;
            
            // Get node driver.
            var driver = _driverRegistry.GetDriver(node);

            if (driver == null)
            {
                _logger.LogWarning("No driver found for node {NodeType}", node.GetType());
                return;
            }

            // Execute driver.
            var result = await driver.ExecuteAsync(context);

            // Execute result.
            await result.ExecuteAsync(context);

            // Invoke next middleware.
            await _next(context);

            if (!context.Bookmarks.Any())
            {
                // Notify node's parent (if any) that its child has executed.
                var owner = context.ScheduledNode.Owner;

                if (owner != null)
                    if (_driverRegistry.GetDriver(owner) is INotifyNodeExecuted parentDriver)
                        await parentDriver.HandleNodeExecuted(context, owner);
            }
        }
    }

    public static class ExecuteNodeMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseNodeDrivers(this INodeExecutionBuilder builder) => builder.UseMiddleware<NodeDriversMiddleware>();
    }
}