using Elsa.Models;

namespace Elsa.Runtime.Contracts
{
    public record WorkflowInstructionResult(WorkflowDefinition WorkflowDefinition, WorkflowExecutionResult WorkflowExecutionResult);
}