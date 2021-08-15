using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Results
{
    public class ScheduleNodesResult : INodeExecutionResult
    {
        public ScheduleNodesResult(IEnumerable<INode> nodes) => Nodes = nodes.ToList();
        public ScheduleNodesResult(params INode[] nodes) => Nodes = nodes;
        public ICollection<INode> Nodes { get; }
        
        public ValueTask ExecuteAsync(NodeExecutionContext context)
        {
            context.ScheduleNodes(Nodes);
            return new ValueTask();
        }
    }
}