using Elsa.Contracts;

namespace Elsa.Runtime.Models
{
    /// <summary>
    /// Represents a workflow definition.
    /// </summary>
    public class WorkflowDefinition
    {
        public string Id { get; set; } = default!;
        public int Version { get; set; } = 1;
        public IActivity Root { get; set; } = default!;
    }
}