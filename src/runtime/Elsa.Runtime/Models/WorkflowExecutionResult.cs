using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Runtime.Models
{
    public record WorkflowExecutionResult(Workflow WorkflowDefinition, WorkflowInstance WorkflowInstance);
}