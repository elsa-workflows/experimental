using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;

namespace Elsa.Services
{
    public class NodeInvoker : INodeInvoker
    {
        private readonly INodeExecutionPipeline _pipeline;
        private readonly INodeSchedulerFactory _schedulerFactory;
        private readonly INodeWalker _nodeWalker;

        public NodeInvoker(INodeExecutionPipeline pipeline, INodeSchedulerFactory schedulerFactory, INodeWalker nodeWalker)
        {
            _pipeline = pipeline;
            _schedulerFactory = schedulerFactory;
            _nodeWalker = nodeWalker;
        }

        public async Task<WorkflowExecutionContext> InvokeAsync(INode node, CancellationToken cancellationToken = default)
        {
            var graph = _nodeWalker.Walk(node).Flatten().ToList();
            var scheduler = _schedulerFactory.CreateScheduler();
            scheduler.Schedule(new ScheduledNode(node));
            var workflowExecutionContext = new WorkflowExecutionContext(node, graph, scheduler);

            while (scheduler.HasAny)
            {
                var currentNode = scheduler.Unschedule();
                var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentNode, cancellationToken);
                await _pipeline.ExecuteAsync(nodeExecutionContext);
            }
            
            // Handle bookmarks.
            
            // 

            return workflowExecutionContext;
        }
    }
}