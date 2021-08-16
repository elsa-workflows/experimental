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

        public async Task<WorkflowExecutionContext> InvokeAsync(INode node, CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = new WorkflowExecutionContext(node);
            workflowExecutionContext.ScheduleScope(new Scope(node));

            while (workflowExecutionContext.Scopes.Any())
            {
                var currentScope = workflowExecutionContext.Scopes.Pop();
                workflowExecutionContext.CurrentScope = currentScope;

                while (currentScope.ScheduledNodes.Any())
                {
                    var nextNode = currentScope.ScheduledNodes.Pop();
                    workflowExecutionContext.CurrentNode = nextNode;
                    var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentScope, nextNode, cancellationToken);
                    await _pipeline.ExecuteAsync(nodeExecutionContext);

                    // If a bookmark is set, stop processing current scope.
                    if (nodeExecutionContext.Bookmark != null)
                    {
                        workflowExecutionContext.AddBookmark(nodeExecutionContext.Bookmark);
                        break;
                    }

                    if (nodeExecutionContext.SuspensionRequested)
                        break;
                }
            }

            return workflowExecutionContext;
        }
    }
}