using Elsa.State;

namespace Elsa.Persistence.Abstractions.Entities;

public class WorkflowInstance : Entity
{
    public string DefinitionId { get; set; } = default!;
    public int Version { get; set; }
    public WorkflowState WorkflowState { get; set; } = default!;
}