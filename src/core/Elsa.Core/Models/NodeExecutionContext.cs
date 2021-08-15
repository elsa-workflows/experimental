using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class NodeExecutionContext
    {
        public NodeExecutionContext(WorkflowExecutionContext workflowExecutionContext, INode node)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            Node = node;
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public INode Node { get; }
        public void ScheduleNode(INode node) => WorkflowExecutionContext.ScheduleNode(node);
        public void ScheduleNodes(IEnumerable<INode> nodes) => WorkflowExecutionContext.ScheduleNodes(nodes);
        public void ScheduleNodes(params INode[] nodes) => WorkflowExecutionContext.ScheduleNodes(nodes);
    }
}