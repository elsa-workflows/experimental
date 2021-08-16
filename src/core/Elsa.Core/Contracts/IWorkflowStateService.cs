using Elsa.Models;
using Elsa.Models.State;

namespace Elsa.Contracts
{
    public interface IWorkflowStateService
    {
        WorkflowState CreateState(WorkflowExecutionContext workflowExecutionContext);
    }
}