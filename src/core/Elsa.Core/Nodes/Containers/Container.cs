using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Nodes.Containers
{
    public abstract class Container : INode
    {
        public ICollection<INode> Nodes { get; set; } = new List<INode>();
    }
}