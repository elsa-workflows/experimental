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
            GetRegisters(state, workflowExecutionContext);
            GetActivityExecutionContexts(state, workflowExecutionContext);

            return state;
        }

        public void WriteState(WorkflowExecutionContext workflowExecutionContext, WorkflowState state)
        {
            workflowExecutionContext.Id = state.Id;
            SetOutput(state, workflowExecutionContext);
            SetCompletionCallbacks(state, workflowExecutionContext);
            SetRegisters(state, workflowExecutionContext);
            SetActivityExecutionContexts(state, workflowExecutionContext);
        }

        private void GetOutput(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            foreach (var node in workflowExecutionContext.Nodes)
                GetOutput(state, node);
        }

        private void GetOutput(WorkflowState state, ActivityNode activityNode)
        {
            var output = GetOutputFrom(activityNode);

            if (output.Any())
                state.ActivityOutput.Add(activityNode.NodeId, output);
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
            foreach (var completionCallbackEntry in state.CompletionCallbacks)
            {
                var nodeId = completionCallbackEntry.Key;
                var node = workflowExecutionContext.FindNodeById(nodeId);
                var activity = node.Activity;
                var callbackName = completionCallbackEntry.Value;
                var callbackDelegate = activity.GetActivityCompletionCallback(callbackName);
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

        private void GetRegisters(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var registers = workflowExecutionContext.Registers;
            var tuples = registers.Select(r => (r.Key, r.Value, new RegisterState(r.Value.Locations.ToDictionary(x => x.Key, x => x.Value)))).ToList();

            foreach (var tuple in tuples)
            {
                var activity = tuple.Key;
                var register = tuple.Value;
                var registerState = tuple.Item3;
                var activityId = activity.ActivityId;
                var parentRegisterState = register.ParentRegister != null ? tuples.First(x => x.Value == register.ParentRegister).Item3 : default;

                registerState.ParentRegister = parentRegisterState;
                state.Registers[activityId] = registerState;
            }
        }

        private void SetRegisters(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var registerStateDictionary = state.Registers;
            var tuples = registerStateDictionary.Select(x => (x.Key, x.Value, new Register(null, x.Value.Locations))).ToList();

            foreach (var tuple in tuples)
            {
                var activityId = tuple.Key;
                var activity = workflowExecutionContext.FindActivityById(activityId);
                var registerState = tuple.Value;
                var register = tuple.Item3;
                var parentRegister = registerState.ParentRegister != null ? tuples.First(x => x.Value == registerState.ParentRegister).Item3 : default;

                register.ParentRegister = parentRegister;
                workflowExecutionContext.Registers[activity] = register;
            }
        }

        private void GetActivityExecutionContexts(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var activityExecutionContexts = workflowExecutionContext.ActivityExecutionContexts;

            var activityExecutionContextStates =
                from activityExecutionContext in activityExecutionContexts
                let activityExecutionContextState = new ActivityExecutionContextState
                {
                    ScheduledActivityId = activityExecutionContext.ScheduledActivity.Activity.ActivityId,
                    Properties = activityExecutionContext.Properties,
                    ExecuteDelegateMethodName = activityExecutionContext.ExecuteDelegate?.Method.Name
                }
                select activityExecutionContextState;
            
            state.ActivityExecutionContexts = new Stack<ActivityExecutionContextState>(activityExecutionContextStates);
        }

        private void SetActivityExecutionContexts(WorkflowState state, WorkflowExecutionContext workflowExecutionContext)
        {
            var activityExecutionContextStates = state.ActivityExecutionContexts;

            var activityExecutionContexts =
                from activityExecutionContextState in activityExecutionContextStates
                let activity = workflowExecutionContext.FindActivityById(activityExecutionContextState.ScheduledActivityId)
                let scheduledActivity = new ScheduledActivity(activity)
                let register = workflowExecutionContext.GetOrCreateRegister(activity)
                let expressionExecutionContext = new ExpressionExecutionContext(register)
                let activityExecutionContext = new ActivityExecutionContext(workflowExecutionContext, expressionExecutionContext, scheduledActivity, workflowExecutionContext.CancellationToken)
                select activityExecutionContext;

            workflowExecutionContext.ActivityExecutionContexts = new List<ActivityExecutionContext>(activityExecutionContexts);
        }

        private IDictionary<string, object?> GetOutputFrom(ActivityNode activityNode) =>
            activityNode.GetType().GetProperties(BindingFlags.Public).Where(x => x.GetCustomAttribute<OutputAttribute>() != null).ToDictionary(x => x.Name, x => (object?)x.GetValue(activityNode));
    }
}