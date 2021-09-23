using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Instructions
{
    public record TriggerWorkflowInstruction(WorkflowTrigger WorkflowTrigger) : IWorkflowInstruction;

    public class TriggerWorkflowInstructionHandler : WorkflowInstructionHandler<TriggerWorkflowInstruction>
    {
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger _logger;

        public TriggerWorkflowInstructionHandler(IWorkflowInvoker workflowInvoker, IWorkflowRegistry workflowRegistry, ILogger<TriggerWorkflowInstructionHandler> logger)
        {
            _workflowInvoker = workflowInvoker;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }

        protected override async ValueTask<WorkflowInstructionResult?> ExecuteInstructionAsync(TriggerWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowTrigger = instruction.WorkflowTrigger;
            var workflowDefinitionId = workflowTrigger.WorkflowDefinitionId;

            // Get workflow to execute.
            var workflow = await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken);

            if (workflow == null)
            {
                _logger.LogWarning("Could not trigger workflow definition {WorkflowDefinitionId} because it was not found", workflowDefinitionId);
                return null;
            }

            // Execute workflow.
            var workflowExecutionResult = await _workflowInvoker.InvokeAsync(workflow, cancellationToken);
            
            return new WorkflowInstructionResult(workflow, workflowExecutionResult);
        }
    }
}