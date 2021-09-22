using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;

namespace Elsa.Runtime.Models
{
    public class WorkflowIndexingContext
    {
        private readonly IList<Node> _nodes;

        public WorkflowIndexingContext(
            Workflow workflow, 
            Node graph,
            CancellationToken cancellationToken)
        {
            Workflow = workflow;
            Graph = graph;
            _nodes = graph.Flatten().ToList();
            CancellationToken = cancellationToken;
            NodeIdLookup = _nodes.ToDictionary(x => x.NodeId);
            NodeActivityLookup = _nodes.ToDictionary(x => x.Activity);
        }

        public Workflow Workflow { get; }
        public Node Graph { get; set; }
        public IReadOnlyCollection<Node> Nodes => new ReadOnlyCollection<Node>(_nodes);
        public IDictionary<string, Node> NodeIdLookup { get; }
        public IDictionary<IActivity, Node> NodeActivityLookup { get; }
        public CancellationToken CancellationToken { get; }
        public IDictionary<IActivity, Register> Registers { get; } = new Dictionary<IActivity, Register>();
        public Node FindNodeById(string nodeId) => NodeIdLookup[nodeId];
        public Node FindNodeByActivity(IActivity activity) => NodeActivityLookup[activity];
        public IActivity FindActivityById(string activityId) => FindNodeById(activityId).Activity;
        
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