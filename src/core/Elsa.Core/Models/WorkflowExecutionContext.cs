using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Elsa.Contracts;
using Elsa.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IList<Node> _nodes;
        private IList<Bookmark> _bookmarks = new List<Bookmark>();

        public WorkflowExecutionContext(IServiceProvider serviceProvider, Node graph, IActivityScheduler scheduler, Trigger? trigger)
        {
            _serviceProvider = serviceProvider;
            Graph = graph;
            _nodes = graph.Flatten().ToList();
            Scheduler = scheduler;
            Trigger = trigger;
            NodeIdLookup = _nodes.ToDictionary(x => x.NodeId);
            NodeActivityLookup = _nodes.ToDictionary(x => x.Activity);
        }

        public Node Graph { get; set; }
        public IReadOnlyCollection<Node> Nodes => new ReadOnlyCollection<Node>(_nodes);
        public IDictionary<string, Node> NodeIdLookup { get; }
        public IDictionary<IActivity, Node> NodeActivityLookup { get; }
        public IActivityScheduler Scheduler { get; }
        public Trigger? Trigger { get; }
        public IReadOnlyCollection<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public T GetRequiredService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);
        public void Schedule(IActivity activity) => Scheduler.Schedule(new ScheduledActivity(activity));
        public Node FindNodeById(string nodeId) => NodeIdLookup[nodeId];
        public Node FindNodeByActivity(IActivity activity) => NodeActivityLookup[activity];
        public IActivity FindActivityById(string activityId) => FindNodeById(activityId).Activity;
        public void SetBookmarks(IEnumerable<Bookmark> bookmarks) => _bookmarks = bookmarks.ToList();
    }
}