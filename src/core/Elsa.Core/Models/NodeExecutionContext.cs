using System.Collections.Generic;
using System.Threading;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class NodeExecutionContext
    {
        public NodeExecutionContext(WorkflowExecutionContext workflowExecutionContext, ScopedExecutionContext scopedExecutionContext, ScheduledNode scheduledNode, CancellationToken cancellationToken)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            ScopedExecutionContext = scopedExecutionContext;
            ScheduledNode = scheduledNode;
            CancellationToken = cancellationToken;
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public ScopedExecutionContext ScopedExecutionContext { get; }
        public Stack<ScheduledNode> ScheduledNodes => ScopedExecutionContext.ScheduledNodes;
        public ScheduledNode ScheduledNode { get; set; }
        public CancellationToken CancellationToken { get; }
        public INode Node => ScheduledNode.Node;

        public void ScheduleNode(INode node, INode? owner = default, IDictionary<string, object?>? variables = default)
        {
            if (owner == null)
            {
                ScheduledNodes.Push(new ScheduledNode(node));
                return;
            }

            var scope = new ScopedExecutionContext(owner, ScopedExecutionContext, variables);
            scope.ScheduledNodes.Push(new ScheduledNode(node));
            WorkflowExecutionContext.ScheduleScope(scope);
        }

        public void ScheduleNodes(params INode[] nodes) => ScheduleNodes((IEnumerable<INode>)nodes);

        public void ScheduleNodes(IEnumerable<INode> nodes, INode? owner = default, IDictionary<string, object?>? variables = default)
        {
            if (owner == null)
            {
                foreach (var node in nodes)
                    ScheduleNode(node);

                return;
            }

            var scope = new ScopedExecutionContext(owner, ScopedExecutionContext, variables);

            foreach (var node in nodes)
                scope.ScheduledNodes.Push(new ScheduledNode(node));

            WorkflowExecutionContext.ScheduleScope(scope);
        }

        public T? GetVariable<T>(string name) => (T?)GetVariable(name);

        public object? GetVariable(string name)
        {
            var current = ScopedExecutionContext;

            while (current != null)
            {
                if (current.Variables.TryGetValue(name, out var value))
                    return value;

                current = current.Parent;
            }

            return default;
        }
    }
}