using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class ActivityNode
    {
        public ActivityNode(IActivity activity)
        {
            Activity = activity;
        }

        public string NodeId => Activity.ActivityId;
        public IActivity Activity { get; }
        public ICollection<ActivityNode> Parents { get; set; } = new List<ActivityNode>();
        public ICollection<ActivityNode> Children { get; set; } = new List<ActivityNode>();

        public IEnumerable<ActivityNode> Descendants()
        {
            foreach (var child in Children)
            {
                yield return child;
                
                var descendants = child.Descendants();

                foreach (var descendant in descendants)
                    yield return descendant;
            }
        }
    }
}