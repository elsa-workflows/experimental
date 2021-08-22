using System.Collections.Generic;

namespace Elsa.Models.State
{
    public class WorkflowState
    {
        public IDictionary<string, IDictionary<string, object?>> ActivityOutput { get; set; } = new Dictionary<string, IDictionary<string, object?>>();
        public ICollection<BookmarkState> Bookmarks { get; set; } = new List<BookmarkState>();
        public IDictionary<string, string> CompletionCallbacks { get; set; } = new Dictionary<string, string>();
    }
}