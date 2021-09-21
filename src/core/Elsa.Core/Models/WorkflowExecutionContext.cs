using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Elsa.Contracts;
using Elsa.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IList<Node> _nodes;
        private readonly IDictionary<IActivity, ActivityCompletionCallback> _completionCallbacks = new Dictionary<IActivity, ActivityCompletionCallback>();
        private IList<Bookmark> _bookmarks = new List<Bookmark>();

        public WorkflowExecutionContext(
            IServiceProvider serviceProvider,
            Workflow workflow, Node graph,
            IActivityScheduler scheduler,
            Trigger? trigger,
            Bookmark? bookmark,
            ExecuteActivityDelegate? executeDelegate,
            CancellationToken cancellationToken)
        {
            _serviceProvider = serviceProvider;
            Workflow = workflow;
            Graph = graph;
            Id = Guid.NewGuid().ToString("N");
            _nodes = graph.Flatten().ToList();
            Scheduler = scheduler;
            Trigger = trigger;
            Bookmark = bookmark;
            ExecuteDelegate = executeDelegate;
            CancellationToken = cancellationToken;
            NodeIdLookup = _nodes.ToDictionary(x => x.NodeId);
            NodeActivityLookup = _nodes.ToDictionary(x => x.Activity);
        }

        public Workflow Workflow { get; }
        public Node Graph { get; set; }
        public string Id { get; set; }
        public IReadOnlyCollection<Node> Nodes => new ReadOnlyCollection<Node>(_nodes);
        public IDictionary<string, Node> NodeIdLookup { get; }
        public IDictionary<IActivity, Node> NodeActivityLookup { get; }
        public IActivityScheduler Scheduler { get; }
        public Trigger? Trigger { get; }
        public Bookmark? Bookmark { get; }
        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();
        public ExecuteActivityDelegate? ExecuteDelegate { get; set; }
        public CancellationToken CancellationToken { get; }
        public IReadOnlyCollection<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public IReadOnlyDictionary<IActivity, ActivityCompletionCallback> CompletionCallbacks => new ReadOnlyDictionary<IActivity, ActivityCompletionCallback>(_completionCallbacks);
        public IDictionary<IActivity, Register> Registers { get; } = new Dictionary<IActivity, Register>();
        public Stack<ActivityExecutionContext> ActivityExecutionContexts { get; set; } = new();
        public IActivity? CurrentActivity { get; set; }

        public T GetRequiredService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);

        public void Schedule(IActivity activity, IActivity owner, ActivityCompletionCallback? completionCallback = default)
        {
            Scheduler.Push(new ScheduledActivity(activity));

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
        public T? GetProperty<T>(string key) => Properties.TryGetValue(key, out var value) ? (T?)value : default(T);
        public void SetProperty<T>(string key, T value) => Properties[key] = value;

        public Register GetOrCreateRegister(IActivity activity)
        {
            if (!Registers.TryGetValue(activity, out var register))
            {
                var activityNode = FindNodeById(activity.ActivityId);
                var parentActivityNode = activityNode.Parent;
                var parentRegister = parentActivityNode != null ? Registers.TryGetValue(parentActivityNode.Activity, out var parent) ? parent : default : default;
                register = new Register(parentRegister);

                Registers[activity] = register;
            }

            return register;
        }

        public void RemoveRegister(IActivity activity) => Registers.Remove(activity);
    }
}