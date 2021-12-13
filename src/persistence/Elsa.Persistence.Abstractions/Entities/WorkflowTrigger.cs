namespace Elsa.Persistence.Abstractions.Entities;

public class WorkflowTrigger : Entity
{
    public string WorkflowId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Hash { get; set; }
}