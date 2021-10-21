using Elsa.Models;

namespace Elsa.Runtime.Contracts
{
    public record ExecuteWorkflowInstructionResult(WorkflowDefinition WorkflowDefinition, ExecuteWorkflowResult ExecuteWorkflowResult);
}