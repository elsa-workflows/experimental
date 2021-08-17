using System.Collections.Generic;
using System.Linq;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Nodes.ControlFlow
{
    public class Fork : Node
    {
        [Ports] public ICollection<INode> Branches { get; set; } = new List<INode>();
    }

    public class ForkDriver : NodeDriver<Fork>
    {
        protected override void Execute(Fork node, NodeExecutionContext context) => context.ScheduleNodes(node.Branches.Reverse());
    }
}