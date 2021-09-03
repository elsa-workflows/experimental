using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IActivityInvoker
    {
        Task<WorkflowExecutionResult> ResumeAsync(string bookmarkName, IActivity root, WorkflowState workflowState, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> InvokeAsync(IActivity activity, IActivity? root = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default);
    }
}