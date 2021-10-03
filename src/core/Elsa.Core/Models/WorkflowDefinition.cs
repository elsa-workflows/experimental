using System;

namespace Elsa.Models
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
        public Workflow Workflow { get; set; } = default!;
    }
}