using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IWorkflowStateService
    {
        WorkflowState ReadState(WorkflowExecutionContext workflowExecutionContext);
        void WriteState(WorkflowExecutionContext workflowExecutionContext, WorkflowState state);
    }
}