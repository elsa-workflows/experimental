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

            // Complete parent chain.
            await CompleteParentsAsync(context, node);
        }

        private static async Task CompleteParentsAsync(NodeExecutionContext context, INode node)
        {
            var graph = context.WorkflowExecutionContext.Graph.ToDictionary(x => x.Node);
            var graphNode = graph[node];
            var currentParent = graphNode.Parent;
            var currentChildContext = context;

            while (currentParent != null)
            {
                var hasScheduledChildren = context.WorkflowExecutionContext.Scheduler.List().Any(x => graph[x.Node].Parent?.Node == node);

                if (!hasScheduledChildren)
                {
                    // Invoke the completion callback, if any.
                    var parentNode = currentParent.Node;
                    var completionCallback = currentChildContext.WorkflowExecutionContext.PopCompletionCallback(parentNode);

                    if (completionCallback != null)
                        await completionCallback.Invoke(currentChildContext, parentNode);
                }

                // Do not continue completion callbacks of parents while there are scheduled nodes.
                if (context.WorkflowExecutionContext.Scheduler.HasAny)
                    break;

                currentChildContext = new NodeExecutionContext(currentChildContext.WorkflowExecutionContext, new ScheduledNode(currentParent.Node), default, currentChildContext.CancellationToken);
                currentParent = currentParent.Parent;
            }
        }
    }

    public static class InvokeDriversMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseNodeDrivers(this INodeExecutionBuilder builder) => builder.UseMiddleware<NodeDriversMiddleware>();
    }
}