using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.Models.State;

namespace Elsa.Services
{
    public class NodeInvoker : INodeInvoker
    {
        private static ValueTask Noop(NodeExecutionContext context) => new();
        
        private readonly INodeExecutionPipeline _pipeline;
        private readonly INodeSchedulerFactory _schedulerFactory;
        private readonly IIdentityGraphService _identityGraphService;
        private readonly IWorkflowStateService _workflowStateService;

        public NodeInvoker(INodeExecutionPipeline pipeline, INodeSchedulerFactory schedulerFactory, IIdentityGraphService identityGraphService, IWorkflowStateService workflowStateService)
        {
            _pipeline = pipeline;
            _schedulerFactory = schedulerFactory;
            _identityGraphService = identityGraphService;
            _workflowStateService = workflowStateService;
        }

        public async Task<WorkflowExecutionContext> ResumeAsync(string bookmarkName, INode root, WorkflowState workflowState, CancellationToken cancellationToken = default)
        {
            var workflowExecutionContext = CreateWorkflowExecutionContext(root, workflowState);
            var bookmark = workflowExecutionContext.PopBookmark(bookmarkName);

            if (bookmark == null)
                return workflowExecutionContext;
            
            // Schedule the node to resume.
            workflowExecutionContext.Scheduler.Schedule(bookmark.Target);

            // If no resumption point was specified, use Noop to prevent the regular "ExecuteAsync" method to be invoked.
            var executeDelegate = bookmark.Resume ?? Noop;
            return await InvokeAsync(workflowExecutionContext, executeDelegate, cancellationToken);
        }

        public async Task<WorkflowExecutionContext> InvokeAsync(INode node, INode? root = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            var currentNode = new ScheduledNode(node);
            return await InvokeAsync(currentNode, root, null, executeNodeDelegate, cancellationToken);
        }

        private async Task<WorkflowExecutionContext> InvokeAsync(ScheduledNode scheduledNode, INode? root = default, WorkflowState? workflowState = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            // If no root was provided, it means the node *is* the root node.
            root ??= scheduledNode.Node;
            
            // Setup a workflow execution context.
            var workflowExecutionContext = CreateWorkflowExecutionContext(root, workflowState);
            
            // Schedule the first node.
            workflowExecutionContext.Scheduler.Schedule(scheduledNode);

            return await InvokeAsync(workflowExecutionContext, executeNodeDelegate, cancellationToken);
        }

        private WorkflowExecutionContext CreateWorkflowExecutionContext(INode root, WorkflowState? workflowState = default)
        {
            // Assign identities.
            var identityGraph = _identityGraphService.AssignIdentities(root);

            // Build a flattened graph of all nodes in the workflow.
            var graph = identityGraph.Select(x => x.Node).ToList();

            // Create scheduler.
            var scheduler = _schedulerFactory.CreateScheduler();

            // Setup a workflow execution context.
            var workflowExecutionContext = new WorkflowExecutionContext(root, graph, scheduler);
            
            // Restore workflow execution context from state, if provided.
            if (workflowState != null)
                _workflowStateService.ApplyState(workflowExecutionContext, workflowState);

            return workflowExecutionContext;
        }

        private async Task<WorkflowExecutionContext> InvokeAsync(WorkflowExecutionContext workflowExecutionContext, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default)
        {
            var scheduler = workflowExecutionContext.Scheduler;
            
            // As long as there are nodes scheduled, keep executing them.
            while (scheduler.HasAny)
            {
                // Pop next node for execution.
                var currentNode = scheduler.Unschedule();

                // Setup a node execution context.
                var nodeExecutionContext = new NodeExecutionContext(workflowExecutionContext, currentNode, executeNodeDelegate, cancellationToken);

                // Execute the node execution pipeline.
                await _pipeline.ExecuteAsync(nodeExecutionContext);

                // Reset custom node execution delegate. This is used only once for the initial node being executed.
                executeNodeDelegate = null;
            }

            return workflowExecutionContext;
        }
    }
}