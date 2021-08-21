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
        
        public async Task<WorkflowExecutionContext> ResumeAsync(Bookmark bookmark, INode root, CancellationToken cancellationToken = default)
        {
            var scheduledNode = bookmark.Target;
            var executeNodeDelegate = bookmark.Resume;
            return await InvokeAsync(scheduledNode, root, executeNodeDelegate, cancellationToken);
        }

        public async Task<WorkflowExecutionContext> InvokeAsync(INode node, INode? root = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            var currentNode = new ScheduledNode(node);
            return await InvokeAsync(currentNode, root, executeNodeDelegate, cancellationToken);
        }

        public async Task<WorkflowExecutionContext> InvokeAsync(ScheduledNode scheduledNode, INode? root = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            // If no root was provided, it means the node *is* the root node.
            root ??= scheduledNode.Node;
            
            // Build a flattened graph of all nodes in the workflow.
            var graph = _nodeWalker.Walk(root).Flatten().ToList();
            
            // Schedule the specified node for execution.
            var scheduler = _schedulerFactory.CreateScheduler();
            scheduler.Schedule(scheduledNode);
            
            // Setup a workflow execution context.
            var workflowExecutionContext = new WorkflowExecutionContext(root, graph, scheduler);

            return await InvokeAsync(workflowExecutionContext, scheduler, executeNodeDelegate, cancellationToken);
        }

        private async Task<WorkflowExecutionContext> InvokeAsync(WorkflowExecutionContext workflowExecutionContext, INodeScheduler scheduler, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            // As long as there are nodes scheduled, keep executing them.
            while (scheduler.HasAny)
            {
                // Pop next node for execution.
                var currentNode = scheduler.Unschedule();

                // Setup a node execution context.
                var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentNode, executeNodeDelegate, cancellationToken);
                
                // Execute the node execution pipeline (which will invoke the node driver).
                await _pipeline.ExecuteAsync(nodeExecutionContext);

                // Reset custom node execution delegate. This is used only once for the initial node being executed.
                executeNodeDelegate = null;
            }

            return workflowExecutionContext;
        }
    }
}