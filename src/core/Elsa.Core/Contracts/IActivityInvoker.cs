using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IActivityInvoker
    {
        Task<WorkflowExecutionContext> ResumeAsync(string bookmarkName, IActivity root, WorkflowState workflowState, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionContext> InvokeAsync(IActivity activity, IActivity? root = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default);
    }
}