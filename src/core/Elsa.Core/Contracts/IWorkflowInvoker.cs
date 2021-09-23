using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IWorkflowInvoker
    {
        Task<WorkflowExecutionResult> ResumeAsync(Workflow workflow, Bookmark bookmark, WorkflowState workflowState, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> InvokeAsync(Workflow workflow, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> InvokeAsync(WorkflowExecutionContext workflowExecutionContext);

        WorkflowExecutionContext CreateWorkflowExecutionContext(
            IServiceProvider serviceProvider,
            Workflow workflow,
            WorkflowState? workflowState = default,
            Bookmark? bookmark = default,
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default);
    }
}