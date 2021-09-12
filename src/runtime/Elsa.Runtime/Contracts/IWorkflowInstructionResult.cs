using Elsa.Models;

namespace Elsa.Runtime.Contracts
{
    public record WorkflowInstructionResult(Workflow Workflow, WorkflowExecutionResult WorkflowExecutionResult);
}