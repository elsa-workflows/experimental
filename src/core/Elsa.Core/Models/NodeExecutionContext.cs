using System.Collections.Generic;
using System.Threading;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class NodeExecutionContext
    {
        public NodeExecutionContext(WorkflowExecutionContext workflowExecutionContext, ScheduledNode scheduledNode, ExecuteNodeDelegate? executeDelegate, CancellationToken cancellationToken)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            ScheduledNode = scheduledNode;
            ExecuteDelegate = executeDelegate;
            CancellationToken = cancellationToken;
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public ScheduledNode ScheduledNode { get; set; }
        public ExecuteNodeDelegate? ExecuteDelegate { get; }
        public CancellationToken CancellationToken { get; }
        public INode Node => ScheduledNode.Node;
        public IEnumerable<Bookmark> Bookmarks => WorkflowExecutionContext.Bookmarks;
        public void ScheduleNode(INode node, NodeCompletionCallback? completionCallback = default) => WorkflowExecutionContext.Scheduler.Schedule(new ScheduledNode(node, completionCallback));
        public void ScheduleNodes(params INode[] nodes) => ScheduleNodes((IEnumerable<INode>)nodes);

        public void ScheduleNodes(IEnumerable<INode> nodes, NodeCompletionCallback? completionCallback = default)
        {
            foreach (var node in nodes)
                WorkflowExecutionContext.Scheduler.Schedule(new ScheduledNode(node, completionCallback));
        }

        public T? GetVariable<T>(string name) => (T?)GetVariable(name);

        public object? GetVariable(string name)
        {
            return default;
        }

        public void SetBookmark(Bookmark bookmark) => WorkflowExecutionContext.SetBookmark(bookmark);
        public void SetBookmark(string name, IDictionary<string, object?>? data = default, ExecuteNodeDelegate? resume = default) => SetBookmark(new Bookmark(ScheduledNode, name, data ?? new Dictionary<string, object?>(), resume));
    }
}