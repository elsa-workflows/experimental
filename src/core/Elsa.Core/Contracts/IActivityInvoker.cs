using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityInvoker
    {
        Task InvokeAsync(
            WorkflowExecutionContext workflowExecutionContext,
            IActivity activity,
            ActivityExecutionContext? owner = default,
            IEnumerable<RegisterLocationReference>? locationReferences = default);

        Task InvokeAsync(ActivityExecutionContext activityExecutionContext);
    }
}