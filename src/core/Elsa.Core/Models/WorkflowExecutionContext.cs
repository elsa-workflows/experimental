using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Elsa.Contracts;
using Elsa.Extensions;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        private readonly IList<Node> _nodes;
        private readonly IDictionary<IActivity, ActivityCompletionCallback> _completionCallbacks = new Dictionary<IActivity, ActivityCompletionCallback>();
        private IList<Bookmark> _bookmarks = new List<Bookmark>();

        public WorkflowExecutionContext(Node graph, IActivityScheduler scheduler)
        {
            Graph = graph;
            _nodes = graph.Flatten().ToList();
            Scheduler = scheduler;
            NodeIdLookup = _nodes.ToDictionary(x => x.NodeId);
            NodeActivityLookup = _nodes.ToDictionary(x => x.Activity);
        }

        public Node Graph { get; set; }
        public IEnumerable<Node> Nodes => new ReadOnlyCollection<Node>(_nodes);
        public IDictionary<string, Node> NodeIdLookup { get; }
        public IDictionary<IActivity, Node> NodeActivityLookup { get; }
        public IActivityScheduler Scheduler { get; }
        public IEnumerable<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public IReadOnlyDictionary<IActivity, ActivityCompletionCallback> CompletionCallbacks => new ReadOnlyDictionary<IActivity, ActivityCompletionCallback>(_completionCallbacks);
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);
        
        public void Schedule(IActivity activity, IActivity owner, ActivityCompletionCallback? completionCallback = default)
        {
            Scheduler.Schedule(new ScheduledActivity(activity));

            if (completionCallback != null)
                AddCompletionCallback(owner, completionCallback);
        }

        public void AddCompletionCallback(IActivity owner, ActivityCompletionCallback completionCallback) => _completionCallbacks.Add(owner, completionCallback);

        public ActivityCompletionCallback? PopCompletionCallback(IActivity owner)
        {
            if (!_completionCallbacks.TryGetValue(owner, out var callback))
                return default;

            _completionCallbacks.Remove(owner);
            return callback;
        }

        public Node FindNodeById(string nodeId) => NodeIdLookup[nodeId];
        public Node FindNodeByActivity(IActivity activity) => NodeActivityLookup[activity];
        public IActivity FindActivityById(string activityId) => FindNodeById(activityId).Activity;
        
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