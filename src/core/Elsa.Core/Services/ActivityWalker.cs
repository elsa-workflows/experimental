using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityWalker : IActivityWalker
    {
        public Node Walk(IActivity activity)
        {
            var collectedNodes = new HashSet<IActivity>(new[] { activity });
            var graph = new Node(activity);
            WalkRecursive((graph, activity), collectedNodes);
            return graph;
        }

        private void WalkRecursive((Node Node, IActivity Activity) pair, HashSet<IActivity> collectedNodes)
        {
            var ports = GetSinglePorts(pair.Activity).Concat(GetManyPorts(pair.Activity)).ToHashSet();

            foreach (var port in ports)
            {
                var childNode = new Node(port, pair.Node);
                collectedNodes.Add(port);
                pair.Node.Children.Add(childNode);
                WalkRecursive((childNode, port), collectedNodes);
            }
        }

        private IEnumerable<IActivity> GetSinglePorts(IActivity activity)
        {
            var nodeType = activity.GetType();

            var ports =
                from prop in nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where typeof(IActivity).IsAssignableFrom(prop.PropertyType)
                let portAttr = prop.GetCustomAttribute<PortAttribute>()
                where portAttr != null
                select (IActivity)prop.GetValue(activity);

            return ports;
        }

        private IEnumerable<IActivity> GetManyPorts(IActivity activity)
        {
            var nodeType = activity.GetType();

            var ports =
                from prop in nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where typeof(IEnumerable<IActivity>).IsAssignableFrom(prop.PropertyType)
                let portsAttr = prop.GetCustomAttribute<PortsAttribute>()
                where portsAttr != null
                select (IEnumerable<IActivity>)prop.GetValue(activity);

            return ports.SelectMany(x => x);
        }
    }
}