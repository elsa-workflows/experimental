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
        
        public ActivityNode(IActivity activity, ActivityNode? parent = default)
        {
            Activity = activity;
            Parent = parent;
        }

        public string NodeId => Activity.ActivityId;
        public IActivity Activity { get; }
        public ActivityNode? Parent { get; }
        public ICollection<ActivityNode> Children { get; set; } = new List<ActivityNode>();

        public object Descendants()
        {
            throw new System.NotImplementedException();
        }
    }
}