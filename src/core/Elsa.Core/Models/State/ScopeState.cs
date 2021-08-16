using System.Collections.Generic;

namespace Elsa.Models.State
{
    public class ScopeState
    {
        public string OwnerId { get; set; } = default!;
        public ScopeState Parent { get; set; } = default!;
        public Stack<ScheduledNodeState> ScheduledNodes { get; set; } = new();
        public IDictionary<string, object?> Variables { get; set; } = new Dictionary<string, object?>();
    }
}