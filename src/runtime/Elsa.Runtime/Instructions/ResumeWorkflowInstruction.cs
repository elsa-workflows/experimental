using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.InstructionResults;
using Elsa.Runtime.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Instructions
{
    public record ResumeWorkflowInstruction(WorkflowBookmark WorkflowBookmark) : IWorkflowInstruction;
    public record ResumeWorkflowExecutionResult(Workflow Workflow, WorkflowInstance WorkflowInstance, ActivityExecutionResult ActivityExecutionResult, WorkflowBookmark OriginalWorkflowBookmark) : IWorkflowInstructionResult;

    public class ResumeWorkflowInstructionHandler : WorkflowInstructionHandler<ResumeWorkflowInstruction>
    {
        private readonly IActivityInvoker _activityInvoker;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly ILogger _logger;

        public ResumeWorkflowInstructionHandler(IActivityInvoker activityInvoker, IWorkflowRegistry workflowRegistry, IWorkflowInstanceStore workflowInstanceStore, ILogger<ResumeWorkflowInstructionHandler> logger)
        {
            _activityInvoker = activityInvoker;
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
            var bookmark = new Bookmark(workflowBookmark.Name, workflowBookmark.Hash, workflowBookmark.ActivityId, workflowBookmark.Data, workflowBookmark.CallbackMethodName);
            var activityExecutionResult = await _activityInvoker.ResumeAsync(bookmark, workflow.Root, workflowInstance.WorkflowState, cancellationToken);

            // Update workflow instance with new workflow state.
            workflowInstance.WorkflowState = activityExecutionResult.WorkflowState;
            
            return new ResumeWorkflowExecutionResult(workflow, workflowInstance, activityExecutionResult, workflowBookmark);
        }
    }
}