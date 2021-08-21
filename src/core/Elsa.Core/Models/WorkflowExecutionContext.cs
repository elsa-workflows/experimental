using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Elsa.Contracts;
using Elsa.Services;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        private readonly IList<GraphNode> _graph;
        private readonly IList<Bookmark> _bookmarks = new List<Bookmark>();

        public WorkflowExecutionContext(INode root, IEnumerable<GraphNode> graph, INodeScheduler scheduler)
        {
            Root = root;
            //CurrentNode = currentNode;
            _graph = graph.ToList();
            Scheduler = scheduler;
        }

        public INode Root { get; set; }
        public IEnumerable<GraphNode> Graph => new ReadOnlyCollection<GraphNode>(_graph);
        public INodeScheduler Scheduler { get; }
        public IEnumerable<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        //public ScheduledNode CurrentNode { get; set; }
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);
    }
}