using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        private readonly IList<GraphNode> _graph;
        private readonly IDictionary<INode, NodeCompletionCallback> _completionCallbacks = new Dictionary<INode, NodeCompletionCallback>();
        private IList<Bookmark> _bookmarks = new List<Bookmark>();

        public WorkflowExecutionContext(INode root, IEnumerable<GraphNode> graph, INodeScheduler scheduler)
        {
            Root = root;
            _graph = graph.ToList();
            Scheduler = scheduler;
            NodeLookup = _graph.ToDictionary(x => x.NodeId, x => x.Node);
        }

        public INode Root { get; set; }
        public IEnumerable<GraphNode> Graph => new ReadOnlyCollection<GraphNode>(_graph);
        public IDictionary<string, INode> NodeLookup { get; }
        public INodeScheduler Scheduler { get; }
        public IEnumerable<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public IReadOnlyDictionary<INode, NodeCompletionCallback> CompletionCallbacks => new ReadOnlyDictionary<INode, NodeCompletionCallback>(_completionCallbacks);
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);

        public void Schedule(INode node, INode owner, NodeCompletionCallback? completionCallback = default)
        {
            Scheduler.Schedule(new ScheduledNode(node));

            if (completionCallback != null)
                AddCompletionCallback(owner, completionCallback);
        }

        public void AddCompletionCallback(INode owner, NodeCompletionCallback completionCallback) => _completionCallbacks.Add(owner, completionCallback);

        public NodeCompletionCallback? PopCompletionCallback(INode owner)
        {
            if (!_completionCallbacks.TryGetValue(owner, out var callback))
                return default;

            _completionCallbacks.Remove(owner);
            return callback;
        }

        public INode FindNodeById(string nodeId) => NodeLookup[nodeId];
        public void SetBookmarks(IEnumerable<Bookmark> bookmarks) => _bookmarks = bookmarks.ToList();

        public Bookmark? PopBookmark(string name)
        {
            var bookmark = _bookmarks.FirstOrDefault(x => x.Name == name);

            if (bookmark == null)
                return null;

            _bookmarks.Remove(bookmark);
            return bookmark;
        }
    }
}