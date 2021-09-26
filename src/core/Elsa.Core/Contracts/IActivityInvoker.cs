using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityInvoker
    {
        Task InvokeAsync(WorkflowExecutionContext workflowExecutionContext, IActivity activity, IActivity? ownerActivity = default, ExecuteActivityDelegate? executeActivityDelegate = default, CancellationToken cancellationToken = default);
    }
}