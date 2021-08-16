using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;

namespace Elsa.Services
{
    public class NodeWalker : INodeWalker
    {
        public IEnumerable<INode> Walk(INode node)
        {
            var collectedNodes = new HashSet<INode>(new[] { node });
            WalkRecursive(node, collectedNodes);
            return collectedNodes;
        }

        private void WalkRecursive(INode node, HashSet<INode> collectedNodes)
        {
            var ports = GetSinglePorts(node).Concat(GetManyPorts(node)).ToHashSet();

            foreach (var port in ports)
            {
                collectedNodes.Add(port);
                WalkRecursive(port, collectedNodes);
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
}