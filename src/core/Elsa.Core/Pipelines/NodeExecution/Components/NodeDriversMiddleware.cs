using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Pipelines.NodeExecution.Components
{
    public class NodeDriversMiddleware
    {
        private readonly NodeMiddlewareDelegate _next;
        private readonly INodeDriverRegistry _driverRegistry;
        private readonly ILogger _logger;

        public NodeDriversMiddleware(NodeMiddlewareDelegate next, INodeDriverRegistry driverRegistry, ILogger<NodeDriversMiddleware> logger)
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
            
            // Exit if any bookmarks were created.
            if (context.Bookmarks.Any())
                return;
            
            // Invoke the completion callback, if any.
            var graphNode = context.WorkflowExecutionContext.Graph.First(x => x.Node == node);
            var parentNode = graphNode.Parent?.Node;

            if (parentNode != null && context.ScheduledNode.CompletionCallback != null)
                await context.ScheduledNode.CompletionCallback.Invoke(context, parentNode);
        }
    }

    public static class InvokeDriversMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseNodeDrivers(this INodeExecutionBuilder builder) => builder.UseMiddleware<NodeDriversMiddleware>();
    }
}