using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class IdentityGraphService : IIdentityGraphService
    {
        private readonly INodeWalker _nodeWalker;
        private readonly IDictionary<Type, int> _identityCounters = new Dictionary<Type, int>();

        public IdentityGraphService(INodeWalker nodeWalker)
        {
            _nodeWalker = nodeWalker;
        }

        public IEnumerable<NodeIdentity> CreateIdentityGraph(INode root)
        {
            var graph = _nodeWalker.Walk(root);
            return graph.Select(CreateIdentity);
        }

        private NodeIdentity CreateIdentity(INode node)
        {
            if (!string.IsNullOrWhiteSpace(node.Name))
                return new NodeIdentity(node, node.Name);
            
            var type = node.GetType();
            var index = GetNextIndexFor(type);
            var name = $"{Camelize(type.Name)}{index + 1}";
            return new NodeIdentity(node, name);
        }

        private int GetNextIndexFor(Type nodeType)
        {
            if (!_identityCounters.TryGetValue(nodeType, out var index))
            {
                _identityCounters[nodeType] = index;
            }
            else
            {
                index = _identityCounters[nodeType] + 1;
                _identityCounters[nodeType] = index;
            }

            return index;
        }

        private string Camelize(string symbol) => char.ToLowerInvariant(symbol[0]) + symbol[1..];
    }
}