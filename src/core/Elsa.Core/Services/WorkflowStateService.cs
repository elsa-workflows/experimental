using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.Models.State;

namespace Elsa.Services
{
    public class WorkflowStateService : IWorkflowStateService
    {
        private readonly INodeDriverRegistry _nodeDriverRegistry;

        public WorkflowStateService(INodeDriverRegistry nodeDriverRegistry)
        {
            _nodeDriverRegistry = nodeDriverRegistry;
        }
        
        public WorkflowState CreateState(WorkflowExecutionContext workflowExecutionContext)
        {
            var state = new WorkflowState();

            AddOutput(state, workflowExecutionContext);
            AddBookmarks(state, workflowExecutionContext);
            AddCompletionCallbacks(state, workflowExecutionContext);

            return state;
        }

        public void ApplyState(WorkflowExecutionContext workflowExecutionContext, WorkflowState state)
        {
            ApplyOutput(state, workflowExecutionContext);
            ApplyBookmarks(state, workflowExecutionContext);
            ApplyCompletionCallbacks(state, workflowExecutionContext);
        }

        private void AddCompletionCallbacks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var completionCallbacks = workflowExecutionContext.CompletionCallbacks;

            foreach (var entry in completionCallbacks)
            {
                var node = entry.Key;
                state.CompletionCallbacks[node.Name] = entry.Value.Method.Name;
            }
        }
        
        private void ApplyCompletionCallbacks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var completionCallbackEntry in state.CompletionCallbacks)
            {
                var nodeId = completionCallbackEntry.Key;
                var node = workflowExecutionContext.FindNodeById(nodeId);
                var driver = _nodeDriverRegistry.GetDriver(node)!;
                var callbackName = completionCallbackEntry.Value;
                var callbackDelegate = driver.GetNodeCompletionCallback(callbackName);
                workflowExecutionContext.AddCompletionCallback(node, callbackDelegate);
            }
        }

        private void AddBookmarks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var bookmarkStates =
                from bookmark in workflowExecutionContext.Bookmarks
                let targetId = bookmark.Target.Node.Name
                let scheduledNodeState = new ScheduledNodeState(targetId)
                select new BookmarkState(scheduledNodeState, bookmark.Name, bookmark.Data, bookmark.Resume?.Method.Name);

            state.Bookmarks = bookmarkStates.ToList();
        }
        
        private void ApplyBookmarks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var bookmarks =
                from bookmarkState in state.Bookmarks
                let node = workflowExecutionContext.FindNodeById(bookmarkState.ScheduledNode.NodeId)
                let scheduledNode = new ScheduledNode(node)
                let driver = _nodeDriverRegistry.GetDriver(node)
                let resumeDelegate = bookmarkState.ResumeActionName != null ? driver.GetResumeNodeDelegate(bookmarkState.ResumeActionName) : default
                select new Bookmark(scheduledNode, bookmarkState.Name, bookmarkState.Data, resumeDelegate);

            workflowExecutionContext.SetBookmarks(bookmarks);
        }

        private void AddOutput(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var node in workflowExecutionContext.Graph)
                AddOutput(state, node.Node);
        }

        private void AddOutput(WorkflowState state, INode node)
        {
            var output = GetOutputFrom(node);

            if (output.Any())
                state.ActivityOutput.Add(node.Name, output);
        }
        
        private void ApplyOutput(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var nodeEntry in state.ActivityOutput)
            {
                var nodeId = nodeEntry.Key;
                var node = workflowExecutionContext.FindNodeById(nodeId);
                var nodeType = node.GetType();

                foreach (var outputEntry in nodeEntry.Value)
                {
                    var propertyName = outputEntry.Key;
                    var propertyValue = outputEntry.Value;
                    var propertyInfo = nodeType.GetProperty(propertyName, BindingFlags.Public)!;
                    propertyInfo.SetValue(node, propertyValue);
                }
            }
        }

        private IDictionary<string, object?> GetOutputFrom(INode node) =>
            node.GetType().GetProperties(BindingFlags.Public).Where(x => x.GetCustomAttribute<OutputAttribute>() != null).ToDictionary(x => x.Name, x => (object?)x.GetValue(node));
    }
}