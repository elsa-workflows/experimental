using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Runtime.Models
{
    /// <summary>
    /// Represents a workflow definition.
    /// </summary>
    public class WorkflowDefinition
    {
        public string Id { get; set; } = default!;
        public string DefinitionId { get; set; } = default!;
        public int Version { get; set; } = 1;
        public IActivity Root { get; set; } = default!;
        public ICollection<TriggerSource> Triggers { get; set; } = new List<TriggerSource>();
    }
}