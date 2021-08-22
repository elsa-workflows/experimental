using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IWorkflowStateService
    {
        WorkflowState CreateState(WorkflowExecutionContext workflowExecutionContext);
        void ApplyState(WorkflowExecutionContext workflowExecutionContext, WorkflowState state);
    }
}