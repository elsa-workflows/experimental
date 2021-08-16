using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class NodeExecutionContext
    {
        public NodeExecutionContext(WorkflowExecutionContext workflowExecutionContext, Scope scope, ScheduledNode scheduledNode, CancellationToken cancellationToken)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            Scope = scope;
            ScheduledNode = scheduledNode;
            CancellationToken = cancellationToken;
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public Scope Scope { get; }
        public Stack<ScheduledNode> ScheduledNodes => Scope.ScheduledNodes;
        public ScheduledNode ScheduledNode { get; set; }
        public CancellationToken CancellationToken { get; }
        public INode Node => ScheduledNode.Node;
        public Bookmark? Bookmark { get; private set; }
        public bool SuspensionRequested { get; set; }

        public void ScheduleNode(INode node, INode? owner = default, IDictionary<string, object?>? variables = default)
        {
            if (owner == null)
            {
                ScheduledNodes.Push(new ScheduledNode(node));
                return;
            }

            var scope = new Scope(owner, Scope, variables);
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

            var scope = new Scope(owner, Scope, variables);

            foreach (var node in nodes)
                scope.ScheduledNodes.Push(new ScheduledNode(node));

            WorkflowExecutionContext.ScheduleScope(scope);
        }

        public T? GetVariable<T>(string name) => (T?)GetVariable(name);

        public object? GetVariable(string name)
        {
            var current = Scope;

            while (current != null)
            {
                if (current.Variables.TryGetValue(name, out var value))
                    return value;

                current = current.Parent;
            }

            return default;
        }

        public void SetBookmark(Bookmark bookmark) => Bookmark = bookmark;
    }
}