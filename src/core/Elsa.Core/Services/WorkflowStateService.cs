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
            var identityLookup = _identityGraphService.CreateIdentityGraph(root).ToDictionary(x => x.Node, x => x.NodeName);
            var state = new WorkflowState();

            AddOutput(state, identityLookup);
            AddScheduledNodes(state, identityLookup, workflowExecutionContext);
            AddBookmarks(state, identityLookup, workflowExecutionContext);
            SetCurrentNode(state, identityLookup, workflowExecutionContext);
            SetCurrentScope(state, identityLookup, workflowExecutionContext);

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

        private void SetCurrentScope(WorkflowState state, Dictionary<INode, string> identityLookup, WorkflowExecutionContext workflowExecutionContext)
        {
            if (workflowExecutionContext.CurrentScope == null)
                return;

            state.CurrentScope = CreateScopeState(workflowExecutionContext.CurrentScope, identityLookup);
        }

        private void SetCurrentNode(WorkflowState state, Dictionary<INode, string> identityLookup, WorkflowExecutionContext workflowExecutionContext)
        {
            if (workflowExecutionContext.CurrentNode == null)
                return;

            state.CurrentNode = new ScheduledNodeState(identityLookup[workflowExecutionContext.CurrentNode.Node]);
        }

        private void AddScheduledNodes(WorkflowState state, IDictionary<INode, string> identityLookup, WorkflowExecutionContext workflowExecutionContext)
        {
            var scopes = workflowExecutionContext.Scopes.ToList();
            var scopeStates = CreateScopeList(workflowExecutionContext, identityLookup);
            var scopeStateLookup = scopeStates.ToDictionary(x => x.Scope, x => x.ScopeState);

            // Assign parents.
            foreach (var scope in scopes.Where(x => x.Parent != null))
            {
                var scopeState = scopeStateLookup[scope];
                scopeState.Parent = scopeStateLookup[scope.Parent!];
            }

            state.Scopes = new Stack<ScopeState>(scopeStateLookup.Select(x => x.Value));
        }

        private ScopeState CreateScopeState(Scope scope, IDictionary<INode, string> identityLookup)
        {
            var ownerId = identityLookup[scope.Owner];
            var scheduledNodeStates = new Stack<ScheduledNodeState>(scope.ScheduledNodes.Select(x => new ScheduledNodeState(identityLookup[x.Node])));

            return new ScopeState
            {
                Variables = scope.Variables,
                OwnerId = ownerId,
                ScheduledNodes = scheduledNodeStates,
            };
        }
        
        private IEnumerable<(Scope Scope, ScopeState ScopeState)> CreateScopeList(WorkflowExecutionContext workflowExecutionContext, IDictionary<INode, string> identityLookup) =>
            from scope in workflowExecutionContext.Scopes
            let scopeState = CreateScopeState(scope, identityLookup)
            select (scope, scopeState);

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