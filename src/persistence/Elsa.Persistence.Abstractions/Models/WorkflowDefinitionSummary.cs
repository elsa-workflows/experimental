using System;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.Models;

public class WorkflowDefinitionSummary
{
    public string Id { get; set; } = default!;
    public string DefinitionId { get; set; } = default!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Version { get; set; } = 1;
    public bool IsLatest { get; set; }
    public bool IsPublished { get; set; }

    public static WorkflowDefinitionSummary FromDefinition(WorkflowDefinition workflowDefinition) => new()
    {
        Id = workflowDefinition.Id,
        DefinitionId = workflowDefinition.DefinitionId,
        Name = workflowDefinition.Name,
        Version = workflowDefinition.Version,
        IsLatest = workflowDefinition.IsLatest,
        IsPublished = workflowDefinition.IsPublished,
        CreatedAt = workflowDefinition.CreatedAt,
    };
}