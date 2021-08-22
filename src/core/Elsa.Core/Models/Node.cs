using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class Node
    {
        public Node(IActivity activity)
        {
            Activity = activity;
        }
        
        public Node(IActivity activity, Node? parent = default)
        {
            Activity = activity;
            Parent = parent;
        }

        public string NodeId => Activity.ActivityId;
        public IActivity Activity { get; }
        public Node? Parent { get; }
        public ICollection<Node> Children { get; set; } = new List<Node>();
    }
}