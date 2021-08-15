using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Results
{
    public static class NodeExecutionResults
    {
        public static EmptyNodeResult Empty() => new();
        public static ScheduleNodesResult ScheduleNode(INode node) => new(node);
        public static ScheduleNodesResult ScheduleNodes(params INode[] nodes) => new(nodes);
        public static ScheduleNodesResult ScheduleNodes(IEnumerable<INode> nodes) => new(nodes);
    }
}