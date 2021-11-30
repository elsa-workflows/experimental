using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Instructions
{
    public record TriggerWorkflowInstruction(WorkflowTrigger WorkflowTrigger) : IWorkflowInstruction;
}