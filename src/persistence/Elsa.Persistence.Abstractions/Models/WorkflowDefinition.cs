using System;
using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Persistence.Abstractions.Models
{
    /// <summary>
    /// Represents a workflow definition.
    /// </summary>
    public class WorkflowDefinition
    {
        public string Id { get; set; } = default!;
        public string DefinitionId { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public int Version { get; set; } = 1;
        public IActivity Root { get; set; } = default!;
        public ICollection<string> Triggers { get; set; } = new List<string>();
    }
}