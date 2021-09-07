using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.InstructionResults;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Instructions
{
    public record ResumeWorkflowInstruction(WorkflowBookmark WorkflowBookmark) : IWorkflowInstruction;

    public record ResumeWorkflowExecutionResult(Workflow Workflow, WorkflowInstance WorkflowInstance, WorkflowExecutionResult WorkflowExecutionResult, WorkflowBookmark OriginalWorkflowBookmark) : IWorkflowInstructionResult;

    public class ResumeWorkflowInstructionHandler : WorkflowInstructionHandler<ResumeWorkflowInstruction>
    {
        private readonly IWorkflowInvoker _workflowInvoker;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly ILogger _logger;

        public ResumeWorkflowInstructionHandler(IWorkflowInvoker workflowInvoker, IWorkflowRegistry workflowRegistry, IWorkflowInstanceStore workflowInstanceStore, ILogger<ResumeWorkflowInstructionHandler> logger)
        {
            _workflowInvoker = workflowInvoker;
            _workflowRegistry = workflowRegistry;
            _workflowInstanceStore = workflowInstanceStore;
            _logger = logger;
        }

        protected override async ValueTask<IWorkflowInstructionResult> ExecuteInstructionAsync(ResumeWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var workflowBookmark = instruction.WorkflowBookmark;
            var workflowDefinitionId = workflowBookmark.WorkflowDefinitionId;
            var workflowInstanceId = workflowBookmark.WorkflowInstanceId;
            var workflow = await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken);

            if (workflow == null)
            {
                _logger.LogWarning("Workflow bookmark {WorkflowBookmarkId} points to workflow definition ID {WorkflowDefinitionId}, but no such workflow definition was found", workflowBookmark.Id, workflowBookmark.WorkflowDefinitionId);
                return NullInstructionResult.Instance;
            }

            var workflowInstance = await _workflowInstanceStore.GetByIdAsync(workflowInstanceId, cancellationToken);

            if (workflowInstance == null)
            {
                _logger
                    .LogWarning(
                        "Workflow bookmark {WorkflowBookmarkId} for workflow definition {WorkflowDefinitionId} points to workflow instance ID {WorkflowInstanceId}, but no such workflow instance was found", workflowBookmark.Id, workflowBookmark.WorkflowDefinitionId, workflowBookmark.WorkflowInstanceId);

                return NullInstructionResult.Instance;
            }

            // Resume workflow instance.
            var bookmark = new Bookmark(workflowBookmark.Id, workflowBookmark.Name, workflowBookmark.Hash, workflowBookmark.ActivityId, workflowBookmark.Data, workflowBookmark.CallbackMethodName);
            var workflowExecutionResult = await _workflowInvoker.ResumeAsync(workflow, bookmark, workflowInstance.WorkflowState, cancellationToken);

            // Update workflow instance with new workflow state.
            workflowInstance.WorkflowState = workflowExecutionResult.WorkflowState;

            return new ResumeWorkflowExecutionResult(workflow, workflowInstance, workflowExecutionResult, workflowBookmark);
        }
    }
}