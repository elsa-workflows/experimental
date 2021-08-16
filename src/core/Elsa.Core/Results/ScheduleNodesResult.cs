using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Results
{
    public class ScheduleNodesResult : INodeExecutionResult
    {
        public ScheduleNodesResult(IEnumerable<INode> nodes, INode? owner = default, IDictionary<string, object?>? variables = default)
        {
            Nodes = nodes.ToList();
            Owner = owner;
            Variables = variables;
        }
        
        public ICollection<INode> Nodes { get; }
        public INode? Owner { get; set; }
        public IDictionary<string, object?>? Variables { get; }

        public ValueTask ExecuteAsync(NodeExecutionContext context)
        {
            context.ScheduleNodes(Nodes, Owner, Variables);
            return new ValueTask();
        }
    }
}