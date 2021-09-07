using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IWorkflowInvoker
    {
        Task<WorkflowExecutionResult> InvokeAsync(Workflow workflow, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> ResumeAsync(Workflow workflow, Bookmark bookmark, WorkflowState workflowState, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> TriggerAsync(Workflow workflow, Trigger trigger, CancellationToken cancellationToken = default);
    }
}