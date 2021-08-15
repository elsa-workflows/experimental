using System;
using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        public Stack<INode> ScheduledNodes { get; set; } = new();
        public void ScheduleNode(INode node) => ScheduledNodes.Push(node);
        public void ScheduleNodes(params INode[] nodes) => ScheduleNodes((IEnumerable<INode>)nodes);

        public void ScheduleNodes(IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
                ScheduleNode(node);
        }
    }
}