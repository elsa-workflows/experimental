using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;

namespace Elsa.Services
{
    public class NodeWalker : INodeWalker
    {
        public GraphNode Walk(INode node)
        {
            var collectedNodes = new HashSet<INode>(new[] { node });
            var graph = new GraphNode(node, null);
            WalkRecursive(graph, collectedNodes);
            return graph;
        }

        private void WalkRecursive(GraphNode node, HashSet<INode> collectedNodes)
        {
            var ports = GetSinglePorts(node.Node).Concat(GetManyPorts(node.Node)).ToHashSet();

            foreach (var port in ports)
            {
                var childNode = new GraphNode(port, node);
                collectedNodes.Add(port);
                node.Children.Add(childNode);
                WalkRecursive(childNode, collectedNodes);
            }
        }

        private IEnumerable<INode> GetSinglePorts(INode node)
        {
            var nodeType = node.GetType();

            var ports =
                from prop in nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where typeof(INode).IsAssignableFrom(prop.PropertyType)
                let portAttr = prop.GetCustomAttribute<PortAttribute>()
                where portAttr != null
                select (INode)prop.GetValue(node);

            return ports;
        }

        private IEnumerable<INode> GetManyPorts(INode node)
        {
            var nodeType = node.GetType();

            var ports =
                from prop in nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where typeof(IEnumerable<INode>).IsAssignableFrom(prop.PropertyType)
                let portsAttr = prop.GetCustomAttribute<PortsAttribute>()
                where portsAttr != null
                select (IEnumerable<INode>)prop.GetValue(node);

            return ports.SelectMany(x => x);
        }
    }

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
    }
}