using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class NodeInvoker : INodeInvoker
    {
        private readonly INodeExecutionPipeline _pipeline;

        public NodeInvoker(INodeExecutionPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public async Task InvokeAsync(INode node, CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = new WorkflowExecutionContext();

            workflowExecutionContext.ScheduleNode(node);

            while (workflowExecutionContext.ScheduledNodes.Any())
            {
                var currentNode = workflowExecutionContext.ScheduledNodes.Pop();
                var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentNode);
                await _pipeline.ExecuteAsync(nodeExecutionContext);
            }
        }
    }
}