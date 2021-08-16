using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        public WorkflowExecutionContext(INode root)
        {
            Root = root;
        }

        public INode Root { get; set; }
        public Stack<Scope> Scopes { get; set; } = new();
        public Scope? CurrentScope { get; set; }
        public ScheduledNode? CurrentNode { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public void ScheduleScope(Scope scope) => Scopes.Push(scope);
        public void AddBookmark(Bookmark bookmark) => Bookmarks.Add(bookmark);
    }
}