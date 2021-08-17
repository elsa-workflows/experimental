using System.Collections.Generic;
using System.Linq;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.ControlFlow
{
    public class Fork : Node
    {
        [Ports] public ICollection<INode> Branches { get; set; } = new List<INode>();
    }

    public class ForkDriver : NodeDriver<Fork>
    {
        protected override INodeExecutionResult Execute(Fork node, NodeExecutionContext context) => ScheduleNodes(node.Branches.Reverse());
    }
}