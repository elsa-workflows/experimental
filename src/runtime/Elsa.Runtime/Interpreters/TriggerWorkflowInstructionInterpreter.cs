using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Interpreters
{
    public class TriggerWorkflowInstructionInterpreter : WorkflowInstructionInterpreter<TriggerWorkflowInstruction>
    {
        private readonly IWorkflowEngine _workflowEngine;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger _logger;

        public TriggerWorkflowInstructionInterpreter(IWorkflowEngine workflowEngine, IWorkflowRegistry workflowRegistry, ILogger<TriggerWorkflowInstructionInterpreter> logger)
        {
            _workflowEngine = workflowEngine;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }

        protected override async ValueTask<WorkflowInstructionResult?> ExecuteInstructionAsync(TriggerWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowTrigger = instruction.WorkflowTrigger;
            var workflowDefinitionId = workflowTrigger.WorkflowDefinitionId;

            // Get workflow to execute.
            var workflowDefinition = await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken);

            if (workflowDefinition == null)
            {
                _logger.LogWarning("Could not trigger workflow definition {WorkflowDefinitionId} because it was not found", workflowDefinitionId);
                return null;
            }

            // Execute workflow.
            var workflowExecutionResult = await _workflowEngine.ExecuteAsync(workflowDefinition, cancellationToken);
            
            return new WorkflowInstructionResult(workflowDefinition, workflowExecutionResult);
        }
    }
}