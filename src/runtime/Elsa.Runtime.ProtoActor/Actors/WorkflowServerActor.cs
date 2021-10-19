using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;
using Elsa.Runtime.Models;
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
        private readonly IStimulusInterpreter _stimulusInterpreter;
        private readonly IWorkflowDefinitionDispatcher _workflowDefinitionDispatcher;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger<WorkflowServerActor> _logger;

        public WorkflowServerActor(IStimulusInterpreter stimulusInterpreter, IWorkflowDefinitionDispatcher workflowDefinitionDispatcher, IWorkflowRegistry workflowRegistry, ILogger<WorkflowServerActor> logger)
        {
            _stimulusInterpreter = stimulusInterpreter;
            _workflowDefinitionDispatcher = workflowDefinitionDispatcher;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            HandleStimulusRequest m => OnHandleStimulus(context, m),
            _ => Task.CompletedTask
        };

        private async Task OnHandleStimulus(IContext context, HandleStimulusRequest message)
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
            
            context.Respond(new Ack());
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
            
            var cancellationToken = context.CancellationToken;
            await _workflowDefinitionDispatcher.DispatchAsync(new DispatchWorkflowDefinitionRequest(workflowDefinitionId, workflowDefinition.Version), cancellationToken);
        }

        private async ValueTask OnResumeWorkflow(IContext context, ResumeWorkflowInstruction instruction)
        {
            var workflowInstanceId = instruction.WorkflowBookmark.WorkflowInstanceId;
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
             await context.ClusterRequestAsync<Ack>(workflowInstanceId, GrainKinds.WorkflowInstance, message, cancellationToken);
        }
    }
}