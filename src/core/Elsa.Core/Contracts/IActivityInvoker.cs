using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IActivityInvoker
    {
        Task<ActivityExecutionResult> InvokeAsync(IActivity activity, IActivity? root = default, ExecuteActivityDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default);
        Task<ActivityExecutionResult> TriggerAsync(Trigger trigger, IActivity root, CancellationToken cancellationToken = default);
        Task<ActivityExecutionResult> ResumeAsync(Bookmark bookmark, IActivity root, WorkflowState workflowState, CancellationToken cancellationToken = default);
    }
}