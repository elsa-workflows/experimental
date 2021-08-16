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
        private readonly INodeDriverRegistry _nodeDriverRegistry;

        public NodeInvoker(INodeExecutionPipeline pipeline, INodeDriverRegistry nodeDriverRegistry)
        {
            _pipeline = pipeline;
            _nodeDriverRegistry = nodeDriverRegistry;
        }

        public async Task InvokeAsync(INode node, CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = new WorkflowExecutionContext();
            workflowExecutionContext.ScheduleScope(new ScopedExecutionContext(node));

            while (workflowExecutionContext.ScheduledScopes.Any())
            {
                var currentScope = workflowExecutionContext.ScheduledScopes.Pop();

                while (currentScope.ScheduledNodes.Any())
                {
                    var nextNode = currentScope.ScheduledNodes.Pop();
                    var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentScope, nextNode, cancellationToken);
                    await _pipeline.ExecuteAsync(nodeExecutionContext);
                }
            }
        }
    }
}