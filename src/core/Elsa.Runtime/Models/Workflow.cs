using Elsa.Contracts;

namespace Elsa.Runtime.Models
{
    /// <summary>
    /// Represents a workflow definition.
    /// </summary>
    public class Workflow
    {
        public string Id { get; set; } = default!;
        private int Version { get; set; } = 1;
        public IActivity Root { get; set; } = default!;
    }
}