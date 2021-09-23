using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityWalker : IActivityWalker
    {
        private readonly IEnumerable<IActivityNodeResolver> _portResolvers;

        public ActivityWalker(IEnumerable<IActivityNodeResolver> portResolvers)
        {
            _portResolvers = portResolvers.OrderByDescending(x => x.Priority).ToList();
        }
        
        public ActivityNode Walk(IActivity activity)
        {
            var collectedActivities = new HashSet<IActivity>(new[] { activity });
            var graph = new ActivityNode(activity);
            WalkRecursive((graph, activity), collectedActivities);
            return graph;
        }
        
        private void WalkRecursive((ActivityNode Node, IActivity Activity) pair, HashSet<IActivity> collectedActivities)
        {
            WalkPortsRecursive(pair, collectedActivities);
        }

        private void WalkPortsRecursive((ActivityNode Node, IActivity Activity) pair, HashSet<IActivity> collectedActivities)
        {
            var resolver = _portResolvers.FirstOrDefault(x => x.GetSupportsActivity(pair.Activity));
            
            if(resolver == null)
                return;

            var ports = resolver.GetNodes(pair.Activity);

            foreach (var port in ports)
            {
                var childNode = new ActivityNode(port, pair.Node);
                collectedActivities.Add(port);
                pair.Node.Children.Add(childNode);
                WalkRecursive((childNode, port), collectedActivities);
            }
        }
    }
}