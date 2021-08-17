using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Results
{
    public static class NodeExecutionResults
    {
        public static DoneResult Done() => new();
        public static ExecutingResult Executing() => new();
        public static ScheduleNodesResult ScheduleNode(INode node) => new(new[] { node });
        public static ScheduleNodesResult ScheduleNodes(params INode[] nodes) => new(nodes);
        public static ScheduleNodesResult ScheduleNodes(IEnumerable<INode> nodes) => new(nodes);
        public static BookmarkResult Bookmark(string name, IDictionary<string, object?>? data = default, Execute? resume = default) => new(name, data, resume);
    }
}