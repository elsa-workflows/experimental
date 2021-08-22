using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Services
{
    public class WorkflowStateService : IWorkflowStateService
    {
        private readonly IActivityDriverRegistry _activityDriverRegistry;

        public WorkflowStateService(IActivityDriverRegistry activityDriverRegistry)
        {
            _activityDriverRegistry = activityDriverRegistry;
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
                var activity = entry.Key;
                state.CompletionCallbacks[activity.ActivityId] = entry.Value.Method.Name;
            }
        }
        
        private void ApplyCompletionCallbacks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var completionCallbackEntry in state.CompletionCallbacks)
            {
                var nodeId = completionCallbackEntry.Key;
                var node = workflowExecutionContext.FindNodeById(nodeId);
                var activity = node.Activity;
                var driver = _activityDriverRegistry.GetDriver(activity)!;
                var callbackName = completionCallbackEntry.Value;
                var callbackDelegate = driver.GetNodeCompletionCallback(callbackName);
                workflowExecutionContext.AddCompletionCallback(activity, callbackDelegate);
            }
        }

        private void AddBookmarks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var bookmarkStates =
                from bookmark in workflowExecutionContext.Bookmarks
                let targetId = bookmark.Target.Activity.ActivityId
                let scheduledNodeState = new ScheduledActivityState(targetId)
                select new BookmarkState(scheduledNodeState, bookmark.Name, bookmark.Data, bookmark.Resume?.Method.Name);

            state.Bookmarks = bookmarkStates.ToList();
        }
        
        private void ApplyBookmarks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var bookmarks =
                from bookmarkState in state.Bookmarks
                let activity = workflowExecutionContext.FindActivityById(bookmarkState.ScheduledActivity.ActivityId)
                let scheduledNode = new ScheduledActivity(activity)
                let driver = _activityDriverRegistry.GetDriver(activity)
                let resumeDelegate = bookmarkState.ResumeActionName != null ? driver.GetResumeNodeDelegate(bookmarkState.ResumeActionName) : default
                select new Bookmark(scheduledNode, bookmarkState.Name, bookmarkState.Data, resumeDelegate);

            workflowExecutionContext.SetBookmarks(bookmarks);
        }

        private void AddOutput(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var node in workflowExecutionContext.Nodes)
                AddOutput(state, node);
        }

        private void AddOutput(WorkflowState state, Node node)
        {
            var output = GetOutputFrom(node);

            if (output.Any())
                state.ActivityOutput.Add(node.NodeId, output);
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

        // private IDictionary<string, object?> GetOutputFrom(INode node) =>
        //     node.GetType().GetProperties(BindingFlags.Public).Where(x => x.GetCustomAttribute<OutputAttribute>() != null).ToDictionary(x => x.Name, x => (object?)x.GetValue(node));
        
        private IDictionary<string, object?> GetOutputFrom(Node node) =>
            node.GetType().GetProperties(BindingFlags.Public).Where(x => x.GetCustomAttribute<OutputAttribute>() != null).ToDictionary(x => x.Name, x => (object?)x.GetValue(node));
    }
}