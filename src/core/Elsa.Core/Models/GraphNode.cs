using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class GraphNode
    {
        public GraphNode(INode node, GraphNode? parent)
        {
            Node = node;
            Parent = parent;
        }

        public INode Node { get; }
        public GraphNode? Parent { get; }
        public ICollection<GraphNode> Children { get; } = new List<GraphNode>();
        public string NodeId => Node.Name;
    }
}