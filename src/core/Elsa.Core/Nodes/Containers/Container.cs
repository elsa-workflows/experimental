using System.Collections.Generic;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Nodes.Containers
{
    public abstract class Container : Node
    {
        [Ports]public ICollection<INode> Nodes { get; set; } = new List<INode>();
        public IDictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();
    }
}