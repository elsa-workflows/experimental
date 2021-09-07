using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.InstructionResults;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Instructions
{
    public record TriggerWorkflowInstruction(WorkflowTrigger WorkflowTrigger) : IWorkflowInstruction;
    public record TriggerWorkflowExecutionResult(Workflow Workflow, WorkflowInstance WorkflowInstance, WorkflowExecutionResult WorkflowExecutionResult) : IWorkflowInstructionResult;

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

        protected override async ValueTask<IWorkflowInstructionResult> ExecuteInstructionAsync(TriggerWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowTrigger = instruction.WorkflowTrigger;
            var trigger = new Trigger(workflowTrigger.Name, workflowTrigger.Hash, workflowTrigger.ActivityId, workflowTrigger.Data);
            var workflowDefinitionId = workflowTrigger.WorkflowDefinitionId;

            // Get workflow to execute.
            var workflow = await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken);

            if (workflow == null)
            {
                _logger.LogWarning("Could not trigger workflow definition {WorkflowDefinitionId} because it was not found", workflowDefinitionId);
                return NullInstructionResult.Instance;
            }

            // Execute workflow.
            var workflowExecutionResult = await _workflowInvoker.TriggerAsync(workflow, trigger, cancellationToken);

            // Create workflow instance.
            var workflowInstance = new WorkflowInstance
            {
                Id = Guid.NewGuid().ToString("N"),
                DefinitionId = workflow.Id,
                Version = workflow.Version,
                WorkflowState = workflowExecutionResult.WorkflowState
            };

            return new TriggerWorkflowExecutionResult(workflow, workflowInstance, workflowExecutionResult);
        }
    }
}