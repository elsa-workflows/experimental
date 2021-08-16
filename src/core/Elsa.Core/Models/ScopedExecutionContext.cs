using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class ScopedExecutionContext
    {
        public ScopedExecutionContext(INode owner, ScopedExecutionContext? parent = default, IDictionary<string, object?>? variables = default)
        {
            Owner = owner;
            Parent = parent;
            Variables = variables ?? new Dictionary<string, object?>();
            ScheduledNodes = new Stack<ScheduledNode>(new[] { new ScheduledNode(owner) });
        }

        public INode Owner { get; set; }
        public ScopedExecutionContext? Parent { get; }
        public Stack<ScheduledNode> ScheduledNodes { get; set; }
        public IDictionary<string, object?> Variables { get; set; }
    }
}