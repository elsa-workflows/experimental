using System;
using Elsa.State;

namespace Elsa.Persistence.Entities;

public class WorkflowInstance : Entity
{
    public string DefinitionId { get; set; } = default!;
    public string DefinitionVersionId { get; set; } = default!;
    public int Version { get; set; }
    public WorkflowState WorkflowState { get; set; } = default!;
    public WorkflowStatus WorkflowStatus { get; set; }
    public string CorrelationId { get; set; } = default!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastExecutedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? FaultedAt { get; set; }
}