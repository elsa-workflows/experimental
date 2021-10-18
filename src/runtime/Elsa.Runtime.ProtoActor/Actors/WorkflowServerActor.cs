using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;
using Elsa.Runtime.ProtoActor.Messages;
using Elsa.Runtime.Stimuli;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;

namespace Elsa.Runtime.ProtoActor.Actors
{
    public class WorkflowServerActor : IActor
    {
        private readonly ActorSystem _actorSystem;
        private readonly IStimulusInterpreter _stimulusInterpreter;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger<WorkflowServerActor> _logger;

        public WorkflowServerActor(ActorSystem actorSystem, IStimulusInterpreter stimulusInterpreter, IWorkflowRegistry workflowRegistry, ILogger<WorkflowServerActor> logger)
        {
            _actorSystem = actorSystem;
            _stimulusInterpreter = stimulusInterpreter;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            HandleStimulus m => OnHandleStimulus(context, m),
            _ => Task.CompletedTask
        };

        private async Task OnHandleStimulus(IContext context, HandleStimulus message)
        {
            var stimulus = new StandardStimulus(
                message.ActivityTypeName,
                message.Hash
            );

            var cancellationToken = context.CancellationToken;
            var instructions = await _stimulusInterpreter.GetExecutionInstructionsAsync(stimulus, cancellationToken);

            foreach (var instruction in instructions)
            {
                await InterpretInstruction(context, instruction);
            }
        }

        private ValueTask InterpretInstruction(IContext context, IWorkflowInstruction instruction) => instruction switch
        {
            TriggerWorkflowInstruction i => OnTriggerWorkflow(context, i),
            ResumeWorkflowInstruction i => OnResumeWorkflow(context, i),
            _ => throw new NotImplementedException()
        };

        private async ValueTask OnTriggerWorkflow(IContext context, TriggerWorkflowInstruction instruction)
        {
            var workflowDefinitionId = instruction.WorkflowTrigger.WorkflowDefinitionId;
            var workflowDefinition = await _workflowRegistry.GetByIdAsync(workflowDefinitionId);

            if (workflowDefinition == null)
            {
                _logger.LogWarning("Could not find workflow definition with ID {WorkflowDefinitionId}", workflowDefinitionId);
                return;
            }

            
            var workflowDefinitionIdAndVersion = $"{workflowDefinition.Id}:{workflowDefinition.Version}";
            var pid = context.System.ProcessRegistry.SearchByName(workflowDefinitionIdAndVersion).FirstOrDefault();

            if (pid == null)
            {
                var props = _actorSystem.DI().PropsFor<WorkflowDefinitionActor>();
                pid = context.SpawnNamed(props, workflowDefinitionIdAndVersion);
            }

            var message = new ExecuteWorkflowDefinition
            {
                Id = workflowDefinitionId,
                Version = workflowDefinition.Version
            };

            //var cancellationToken = context.CancellationToken;
            //var actor = await context.ClusterRequestAsync<WorkflowDefinitionActor>(workflowDefinitionIdAndVersion, "Execute", message, cancellationToken);
            context.Send(pid, message);
        }

        private async ValueTask OnResumeWorkflow(IContext context, ResumeWorkflowInstruction instruction)
        {
            var workflowInstanceId = instruction.WorkflowBookmark.WorkflowInstanceId;
            var props = _actorSystem.DI().PropsFor<WorkflowInstanceActor>();
            var workflowInstancePid = context.SpawnNamed(props, workflowInstanceId);
            var bookmark = instruction.WorkflowBookmark;

            var message = new ExecuteWorkflowInstance
            {
                Id = workflowInstanceId,
                Bookmark = new Bookmark
                {
                    Id = bookmark.Id,
                    Name = bookmark.Name,
                    ActivityId = bookmark.ActivityId,
                    ActivityInstanceId = bookmark.ActivityInstanceId,
                    Hash = bookmark.Hash,
                    CallbackMethodName = bookmark.CallbackMethodName
                }
            };

            var cancellationToken = context.CancellationToken;
            var actor = await context.ClusterRequestAsync<WorkflowInstanceActor>(workflowInstanceId, "Execute", message, cancellationToken);

            //context.Send(workflowInstancePid, message);
        }
    }
}