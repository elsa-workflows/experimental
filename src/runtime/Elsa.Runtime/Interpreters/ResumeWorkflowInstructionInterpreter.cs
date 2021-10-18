using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Interpreters
{
    public class ResumeWorkflowInstructionInterpreter : WorkflowInstructionInterpreter<ResumeWorkflowInstruction>
    {
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly ILogger _logger;

        public ResumeWorkflowInstructionInterpreter(IWorkflowInvoker workflowInvoker, IWorkflowRegistry workflowRegistry, IWorkflowInstanceStore workflowInstanceStore, ILogger<ResumeWorkflowInstructionInterpreter> logger)
        {
            _workflowInvoker = workflowInvoker;
            _workflowRegistry = workflowRegistry;
            _workflowInstanceStore = workflowInstanceStore;
            _logger = logger;
        }

        protected override async ValueTask<WorkflowInstructionResult?> ExecuteInstructionAsync(ResumeWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowBookmark = instruction.WorkflowBookmark;
            var workflowDefinitionId = workflowBookmark.WorkflowDefinitionId;
            var workflowInstanceId = workflowBookmark.WorkflowInstanceId;
            var workflowDefinition = await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken);

            if (workflowDefinition == null)
            {
                _logger.LogWarning("Workflow bookmark {WorkflowBookmarkId} points to workflow definition ID {WorkflowDefinitionId}, but no such workflow definition was found", workflowBookmark.Id, workflowBookmark.WorkflowDefinitionId);
                return null;
            }

            var workflowInstance = await _workflowInstanceStore.GetByIdAsync(workflowInstanceId, cancellationToken);

            if (workflowInstance == null)
            {
                _logger
                    .LogWarning(
                        "Workflow bookmark {WorkflowBookmarkId} for workflow definition {WorkflowDefinitionId} points to workflow instance ID {WorkflowInstanceId}, but no such workflow instance was found", workflowBookmark.Id, workflowBookmark.WorkflowDefinitionId, workflowBookmark.WorkflowInstanceId);

                return null;
            }

            // Resume workflow instance.
            var bookmark = new Bookmark(workflowBookmark.Id, workflowBookmark.Name, workflowBookmark.Hash, workflowBookmark.ActivityId, workflowBookmark.ActivityInstanceId, workflowBookmark.Data, workflowBookmark.CallbackMethodName);
            var workflowState = workflowInstance.WorkflowState;
            var workflowExecutionResult = await _workflowInvoker.InvokeAsync(workflowDefinition, workflowState, bookmark, cancellationToken);

            // Update workflow instance with new workflow state.
            workflowInstance.WorkflowState = workflowExecutionResult.WorkflowState;

            return new WorkflowInstructionResult(workflowDefinition, workflowExecutionResult);
        }
    }
}