using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Services
{
    public class NodeInvoker : INodeInvoker
    {
        private readonly INodeDriverRegistry _driverRegistry;
        private readonly ILogger<NodeInvoker> _logger;

        public NodeInvoker(INodeDriverRegistry driverRegistry, ILogger<NodeInvoker> logger)
        {
            _driverRegistry = driverRegistry;
            _logger = logger;
        }

        public async Task InvokeAsync(INode node, CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = new WorkflowExecutionContext();

            workflowExecutionContext.ScheduleNode(node);

            while (workflowExecutionContext.ScheduledNodes.Any())
            {
                var currentNode = workflowExecutionContext.ScheduledNodes.Pop();
                
                var driver = _driverRegistry.GetDriver(currentNode);

                if (driver == null)
                {
                    _logger.LogWarning("No driver found for node {NodeType}", currentNode.GetType());
                    continue;
                }
                
                var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentNode);
                
                var result = await driver.ExecuteAsync(nodeExecutionContext);
                await result.ExecuteAsync(nodeExecutionContext);
            }
        }
    }
}