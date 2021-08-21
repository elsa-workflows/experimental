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
        private readonly IDictionary<INode, NodeCompletionCallback> _completionCallbacks = new Dictionary<INode, NodeCompletionCallback>();

        public WorkflowExecutionContext(INode root, IEnumerable<GraphNode> graph, INodeScheduler scheduler)
        {
            Root = root;
            _graph = graph.ToList();
            Scheduler = scheduler;
        }

        public INode Root { get; set; }
        public IEnumerable<GraphNode> Graph => new ReadOnlyCollection<GraphNode>(_graph);
        public INodeScheduler Scheduler { get; }
        public IEnumerable<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);

        public void Schedule(INode node, INode owner, NodeCompletionCallback? completionCallback = default)
        {
            Scheduler.Schedule(new ScheduledNode(node));
            
            if(completionCallback != null)
                _completionCallbacks.Add(owner, completionCallback);
        }

        public NodeCompletionCallback? PopCompletionCallback(INode owner)
        {
            if (!_completionCallbacks.TryGetValue(owner, out var callback)) 
                return default;
            
            _completionCallbacks.Remove(owner);
            return callback;

        }
    }
}