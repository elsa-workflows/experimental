using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.State;
using Microsoft.Extensions.Logging;

namespace Elsa.Services
{
    public class WorkflowStateSerializer : IWorkflowStateSerializer
    {
        private readonly ILogger _logger;

        public WorkflowStateSerializer(ILogger<WorkflowStateSerializer> logger)
        {
            _logger = logger;
        }

        public WorkflowState ReadState(WorkflowExecutionContext workflowExecutionContext)
        {
            var state = new WorkflowState
            {
                Id = workflowExecutionContext.Id
            };

            GetOutput(state, workflowExecutionContext);
            GetCompletionCallbacks(state, workflowExecutionContext);

            return state;
        }

        public void WriteState(WorkflowExecutionContext workflowExecutionContext, WorkflowState state)
        {
            workflowExecutionContext.Id = state.Id;
            SetOutput(state, workflowExecutionContext);
            SetCompletionCallbacks(state, workflowExecutionContext);
        }

        private void GetOutput(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var node in workflowExecutionContext.Nodes)
                GetOutput(state, node);
        }

        private void GetOutput(WorkflowState state, Node node)
        {
            var output = GetOutputFrom(node);

            if (output.Any())
                state.ActivityOutput.Add(node.NodeId, output);
        }

        private void SetOutput(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
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
        
        private void SetCompletionCallbacks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var activityDriverActivator = workflowExecutionContext.GetRequiredService<IActivityDriverActivator>();

            foreach (var completionCallbackEntry in state.CompletionCallbacks)
            {
                var nodeId = completionCallbackEntry.Key;
                var node = workflowExecutionContext.FindNodeById(nodeId);
                var activity = node.Activity;
                var driver = activityDriverActivator.ActivateDriver(activity);

                if (driver == null)
                {
                    _logger.LogWarning("No driver found for node {ActivityType}", activity.GetType());
                    continue;
                }

                var callbackName = completionCallbackEntry.Value;
                var callbackDelegate = driver.GetActivityCompletionCallback(callbackName);
                workflowExecutionContext.AddCompletionCallback(activity, callbackDelegate);
            }
        }
        
        private void GetCompletionCallbacks(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var completionCallbacks = workflowExecutionContext.CompletionCallbacks;

            foreach (var entry in completionCallbacks)
            {
                var activity = entry.Key;
                state.CompletionCallbacks[activity.ActivityId] = entry.Value.Method.Name;
            }
        }

        private IDictionary<string, object?> GetOutputFrom(Node node) =>
            node.GetType().GetProperties(BindingFlags.Public).Where(x => x.GetCustomAttribute<OutputAttribute>() != null).ToDictionary(x => x.Name, x => (object?)x.GetValue(node));
    }
}