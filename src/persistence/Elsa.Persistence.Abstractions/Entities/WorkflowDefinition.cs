using System;
using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Persistence.Abstractions.Entities;

/// <summary>
/// Represents a workflow definition.
/// </summary>
public class WorkflowDefinition : Entity
{
    public string DefinitionId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public int Version { get; set; } = 1;
    public IActivity Root { get; set; } = default!;
    public ICollection<ITrigger> Triggers { get; set; } = new List<ITrigger>();
}