using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;

namespace Elsa.Services
{
    public class IdentityGraphService : IIdentityGraphService
    {
        public IEnumerable<NodeIdentity> CreateIdentityGraph(Node root)
        {
            var identityCounters = new Dictionary<string, int>();
            var list = root.Flatten();
            return list.Select(x => CreateIdentity(x, identityCounters));
        }

        public IEnumerable<NodeIdentity> AssignIdentities(Node root)
        {
            var identityGraph = CreateIdentityGraph(root).ToList();

            foreach (var nodeIdentity in identityGraph)
                nodeIdentity.Node.Activity.ActivityId = nodeIdentity.NodeName;

            return identityGraph;
        }

        private NodeIdentity CreateIdentity(Node node, IDictionary<string, int> identityCounters)
        {
            if (!string.IsNullOrWhiteSpace(node.NodeId))
                return new NodeIdentity(node, node.NodeId);

            var type = node.Activity.ActivityType;
            var index = GetNextIndexFor(type, identityCounters);
            var name = $"{Camelize(type)}{index + 1}";
            return new NodeIdentity(node, name);
        }

        private int GetNextIndexFor(string nodeType, IDictionary<string, int> identityCounters)
        {
            if (!identityCounters.TryGetValue(nodeType, out var index))
            {
                identityCounters[nodeType] = index;
            }
            else
            {
                index = identityCounters[nodeType] + 1;
                identityCounters[nodeType] = index;
            }

            return index;
        }

        private string Camelize(string symbol) => char.ToLowerInvariant(symbol[0]) + symbol[1..];
    }
}