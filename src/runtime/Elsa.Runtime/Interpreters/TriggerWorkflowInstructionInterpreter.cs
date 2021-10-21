using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;
using Elsa.Runtime.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Interpreters
{
    public class TriggerWorkflowInstructionInterpreter : WorkflowInstructionInterpreter<TriggerWorkflowInstruction>
    {
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger _logger;

        public TriggerWorkflowInstructionInterpreter(IWorkflowInvoker workflowInvoker, IWorkflowRegistry workflowRegistry, ILogger<TriggerWorkflowInstructionInterpreter> logger)
        {
            _workflowInvoker = workflowInvoker;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }

        protected override async ValueTask<ExecuteWorkflowInstructionResult?> ExecuteInstructionAsync(TriggerWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowTrigger = instruction.WorkflowTrigger;
            var workflowDefinitionId = workflowTrigger.WorkflowDefinitionId;

            // Get workflow to execute.
            var workflowDefinition = await GetWorkflowDefinitionAsync(workflowDefinitionId, cancellationToken);

            if (workflowDefinition == null)
                return null;

            // Execute workflow.
            var executeRequest = new ExecuteWorkflowDefinitionRequest(workflowDefinitionId, workflowDefinition.Version);
            var workflowExecutionResult = await _workflowInvoker.ExecuteAsync(executeRequest, cancellationToken);
            
            return new ExecuteWorkflowInstructionResult(workflowDefinition, workflowExecutionResult);
        }

        protected override async ValueTask<DispatchWorkflowInstructionResult?> DispatchInstructionAsync(TriggerWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowTrigger = instruction.WorkflowTrigger;
            var workflowDefinitionId = workflowTrigger.WorkflowDefinitionId;

            // Get workflow to dispatch.
            var workflowDefinition = await GetWorkflowDefinitionAsync(workflowDefinitionId, cancellationToken);

            if (workflowDefinition == null)
                return null;

            // Execute workflow.
            var dispatchRequest = new DispatchWorkflowDefinitionRequest(workflowDefinitionId, workflowDefinition.Version);
            await _workflowInvoker.DispatchAsync(dispatchRequest, cancellationToken);
            
            return new DispatchWorkflowInstructionResult();
        }

        private async Task<WorkflowDefinition?> GetWorkflowDefinitionAsync(string definitionId, CancellationToken cancellationToken)
        {
            // Get workflow to execute.
            var workflowDefinition = await _workflowRegistry.GetByIdAsync(definitionId, cancellationToken);

            if (workflowDefinition != null) 
                return workflowDefinition;
            
            _logger.LogWarning("Could not trigger workflow definition {WorkflowDefinitionId} because it was not found", definitionId);
            return null;
        }
    }
}