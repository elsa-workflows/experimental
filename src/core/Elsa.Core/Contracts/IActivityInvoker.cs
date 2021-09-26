using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityInvoker
    {
        Task InvokeAsync(
            WorkflowExecutionContext workflowExecutionContext,
            IActivity activity,
            IActivity? ownerActivity = default,
            IEnumerable<RegisterLocationReference>? locationReferences = default,
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default);

        Task InvokeAsync(ActivityExecutionContext activityExecutionContext);
    }
}