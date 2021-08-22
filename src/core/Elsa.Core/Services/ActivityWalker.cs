using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityWalker : IActivityWalker
    {
        private readonly IEnumerable<IActivityPortResolver> _portResolvers;

        public ActivityWalker(IEnumerable<IActivityPortResolver> portResolvers)
        {
            _portResolvers = portResolvers;
        }
        
        public Node Walk(IActivity activity)
        {
            var collectedActivities = new HashSet<IActivity>(new[] { activity });
            var graph = new Node(activity);
            WalkRecursive((graph, activity), collectedActivities);
            return graph;
        }

        private void WalkRecursive((Node Node, IActivity Activity) pair, HashSet<IActivity> collectedActivities)
        {
            var resolver = _portResolvers.FirstOrDefault(x => x.GetSupportsActivity(pair.Activity));
            
            if(resolver == null)
                return;

            var ports = resolver.GetPorts(pair.Activity);

            foreach (var port in ports)
            {
                var childNode = new Node(port, pair.Node);
                collectedActivities.Add(port);
                pair.Node.Children.Add(childNode);
                WalkRecursive((childNode, port), collectedActivities);
            }
        }
    }
}