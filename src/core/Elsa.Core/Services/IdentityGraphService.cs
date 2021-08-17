using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;

namespace Elsa.Services
{
    public class IdentityGraphService : IIdentityGraphService
    {
        private readonly INodeWalker _nodeWalker;

        public IdentityGraphService(INodeWalker nodeWalker)
        {
            _nodeWalker = nodeWalker;
        }

        public IEnumerable<NodeIdentity> CreateIdentityGraph(INode root)
        {
            var identityCounters = new Dictionary<Type, int>();
            var list = _nodeWalker.Walk(root).Flatten();
            return list.Select(x => CreateIdentity(x, identityCounters));
        }

        private NodeIdentity CreateIdentity(GraphNode node, IDictionary<Type, int> identityCounters)
        {
            if (!string.IsNullOrWhiteSpace(node.Node.Name))
                return new NodeIdentity(node, node.Node.Name);

            var type = node.Node.GetType();
            var index = GetNextIndexFor(type, identityCounters);
            var name = $"{Camelize(type.Name)}{index + 1}";
            return new NodeIdentity(node, name);
        }

        private int GetNextIndexFor(Type nodeType, IDictionary<Type, int> identityCounters)
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