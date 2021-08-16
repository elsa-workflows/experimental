using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Results
{
    public static class NodeExecutionResults
    {
        public static DoneResult Done() => new();
        public static ExecutingResult Executing() => new();
        public static ScheduleNodesResult ScheduleNode(INode node, INode? owner = default, IDictionary<string, object?>? variables = default) => new(new[]{node}, owner, variables);
        public static ScheduleNodesResult ScheduleNodes(params INode[] nodes) => new(nodes);
        public static ScheduleNodesResult ScheduleNodes(IEnumerable<INode> nodes, INode? owner = default, IDictionary<string, object?>? variables = default) => new(nodes, owner, variables);
    }
}