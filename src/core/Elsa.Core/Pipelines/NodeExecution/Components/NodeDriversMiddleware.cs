using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.NodeExecution.Components
{
    public class NodeDriversMiddleware
    {
        private readonly ExecuteNodeMiddlewareDelegate _next;
        private readonly INodeDriverRegistry _driverRegistry;
        private readonly ILogger _logger;

        public NodeDriversMiddleware(ExecuteNodeMiddlewareDelegate next, INodeDriverRegistry driverRegistry, ILogger<NodeDriversMiddleware> logger)
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
            var driverType = typeof(INodeDriver);
            var methodInfo = driverType.GetMethod(nameof(INodeDriver.ExecuteAsync))!;
            var executeDelegate = context.ExecuteDelegate ?? (ExecuteNodeDelegate)Delegate.CreateDelegate(typeof(ExecuteNodeDelegate), driver, methodInfo);
            await executeDelegate(context);
            
            // Invoke next middleware.
            await _next(context);
        }
    }

    public static class InvokeDriversMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseNodeDrivers(this INodeExecutionBuilder builder) => builder.UseMiddleware<NodeDriversMiddleware>();
    }
}