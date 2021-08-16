using System.Collections.Generic;

namespace Elsa.Models.State
{
    public class WorkflowState
    {
        public IDictionary<string, IDictionary<string, object?>> ActivityOutput { get; set; } = new Dictionary<string, IDictionary<string, object?>>();
        public Stack<ScopeState> Scopes { get; set; } = new();
        public ScopeState? CurrentScope { get; set; }
        public ScheduledNodeState? CurrentNode { get; set; }
        public ICollection<BookmarkState> Bookmarks { get; set; } = new List<BookmarkState>();
    }
}