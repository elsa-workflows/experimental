using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;

namespace Elsa.Services
{
    public class IdentityGraphService : IIdentityGraphService
    {
        private readonly IActivityWalker _activityWalker;

        public IdentityGraphService(IActivityWalker activityWalker)
        {
            _activityWalker = activityWalker;
        }
        
        public void AssignIdentities(IActivity root)
        {
            var graph = _activityWalker.Walk(root);
            AssignIdentities(graph);
        }
        
        public void AssignIdentities(ActivityNode root)
        {
            var identityCounters = new Dictionary<string, int>();
            var list = root.Flatten();

            foreach (var node in list)
                node.Activity.ActivityId = CreateId(node, identityCounters);
        }
        
        private string CreateId(ActivityNode activityNode, IDictionary<string, int> identityCounters)
        {
            if (!string.IsNullOrWhiteSpace(activityNode.NodeId))
                return activityNode.NodeId;

            var type = activityNode.Activity.ActivityType;
            var index = GetNextIndexFor(type, identityCounters);
            var name = $"{Camelize(type)}{index + 1}";
            return name;
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