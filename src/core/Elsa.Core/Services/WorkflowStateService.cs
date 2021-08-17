using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Models.State;

namespace Elsa.Services
{
    public class WorkflowStateService : IWorkflowStateService
    {
        private readonly IIdentityGraphService _identityGraphService;

        public WorkflowStateService(IIdentityGraphService identityGraphService)
        {
            _identityGraphService = identityGraphService;
        }

        public WorkflowState CreateState(WorkflowExecutionContext workflowExecutionContext)
        {
            var root = workflowExecutionContext.Root;
            var identityLookup = _identityGraphService.CreateIdentityGraph(root).ToDictionary(x => x.Node.Node, x => x.NodeName);
            var state = new WorkflowState();

            AddOutput(state, identityLookup);
            AddScheduledNodes(state, identityLookup, workflowExecutionContext);
            AddBookmarks(state, identityLookup, workflowExecutionContext);

            return state;
        }

        private void AddBookmarks(WorkflowState state, Dictionary<INode,string> identityLookup, WorkflowExecutionContext workflowExecutionContext)
        {
            var bookmarkStates =
                from bookmark in workflowExecutionContext.Bookmarks
                let targetId = identityLookup[bookmark.Target]
                select new BookmarkState(targetId, bookmark.Name, bookmark.Data, bookmark.Resume?.Method.Name);

            state.Bookmarks = bookmarkStates.ToList();
        }

        private void AddScheduledNodes(WorkflowState state, IDictionary<INode, string> identityLookup, WorkflowExecutionContext workflowExecutionContext)
        {
        }
        
        private void AddOutput(WorkflowState state, IDictionary<INode, string> identityLookup)
        {
            foreach (var nodeIdentity in identityLookup)
                AddOutput(state, nodeIdentity.Value, nodeIdentity.Key);
        }

        private void AddOutput(WorkflowState state, string nodeName, INode node)
        {
            var output = GetOutputFrom(node);

            if (output.Any())
                state.ActivityOutput.Add(nodeName, output);
        }

        private IDictionary<string, object?> GetOutputFrom(INode node) =>
            node.GetType().GetProperties(BindingFlags.Public).Where(x => x.GetCustomAttribute<OutputAttribute>() != null).ToDictionary(x => x.Name, x => (object?)x.GetValue(node));
    }
}