using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Elsa.Contracts;
using Elsa.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Models
{
    public record ActivityCompletionCallbackEntry(IActivity Owner, IActivity Child, ActivityCompletionCallback CompletionCallback);

    public class WorkflowExecutionContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IList<ActivityNode> _nodes;
        private readonly IList<ActivityCompletionCallbackEntry> _completionCallbackEntries = new List<ActivityCompletionCallbackEntry>();
        private readonly List<Bookmark> _bookmarks = new();

        public WorkflowExecutionContext(
            IServiceProvider serviceProvider,
            Workflow workflow, ActivityNode graph,
            IActivityScheduler scheduler,
            Bookmark? bookmark,
            ExecuteActivityDelegate? executeDelegate,
            CancellationToken cancellationToken)
        {
            _serviceProvider = serviceProvider;
            Workflow = workflow;
            Graph = graph;
            Id = Guid.NewGuid().ToString("N");
            _nodes = graph.Flatten().Distinct().ToList();
            Scheduler = scheduler;
            Bookmark = bookmark;
            ExecuteDelegate = executeDelegate;
            CancellationToken = cancellationToken;
            NodeIdLookup = _nodes.ToDictionary(x => x.NodeId);
            NodeActivityLookup = _nodes.ToDictionary(x => x.Activity);
        }

        public Workflow Workflow { get; }
        public ActivityNode Graph { get; set; }
        public string Id { get; set; }
        public IReadOnlyCollection<ActivityNode> Nodes => new ReadOnlyCollection<ActivityNode>(_nodes);
        public IDictionary<string, ActivityNode> NodeIdLookup { get; }
        public IDictionary<IActivity, ActivityNode> NodeActivityLookup { get; }
        public IActivityScheduler Scheduler { get; }
        public Bookmark? Bookmark { get; }
        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();
        public ExecuteActivityDelegate? ExecuteDelegate { get; set; }
        public CancellationToken CancellationToken { get; }
        public IReadOnlyCollection<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public IReadOnlyCollection<ActivityCompletionCallbackEntry> CompletionCallbacks => new ReadOnlyCollection<ActivityCompletionCallbackEntry>(_completionCallbackEntries);
        public ICollection<ActivityExecutionContext> ActivityExecutionContexts { get; set; } = new List<ActivityExecutionContext>();
        public T GetRequiredService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

        public ScheduledActivity Schedule(IActivity activity, IActivity owner, ActivityCompletionCallback? completionCallback = default)
        {
            var scheduledActivity = new ScheduledActivity(activity, owner); 
            Scheduler.Push(scheduledActivity);

            if (completionCallback != null)
                AddCompletionCallback(owner, activity, completionCallback);

            return scheduledActivity;
        }

        public void AddCompletionCallback(IActivity owner, IActivity child, ActivityCompletionCallback completionCallback)
        {
            var entry = new ActivityCompletionCallbackEntry(owner, child, completionCallback);
            _completionCallbackEntries.Add(entry);
        }

        public ActivityCompletionCallback? PopCompletionCallback(IActivity owner, IActivity child)
        {
            var entry = _completionCallbackEntries.FirstOrDefault(x => x.Owner == owner && x.Child == child);

            if (entry == null)
                return default;

            _completionCallbackEntries.Remove(entry);
            return entry.CompletionCallback;
        }

        public ActivityNode FindNodeById(string nodeId) => NodeIdLookup[nodeId];
        public ActivityNode FindNodeByActivity(IActivity activity) => NodeActivityLookup[activity];
        public IActivity FindActivityById(string activityId) => FindNodeById(activityId).Activity;
        public T? GetProperty<T>(string key) => Properties.TryGetValue(key, out var value) ? (T?)value : default(T);

        public void SetProperty<T>(string key, T value) => Properties[key] = value;
        
        public void RegisterBookmarks(IEnumerable<Bookmark> bookmarks) => _bookmarks.AddRange(bookmarks);

        public void UnregisterBookmarks(IEnumerable<Bookmark> bookmarks)
        {
            foreach (var bookmark in bookmarks)
                _bookmarks.Remove(bookmark);
        }
    }
}