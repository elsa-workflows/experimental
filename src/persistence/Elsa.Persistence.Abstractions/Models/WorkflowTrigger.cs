using System.Collections.Generic;

namespace Elsa.Persistence.Abstractions.Models
{
    public class WorkflowTrigger
    {
        public string Id { get; set; } = default!;
        public string WorkflowDefinitionId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string ActivityId { get; set; } = default!;
        public string? Hash { get; set; }
        public IDictionary<string, object?> Data { get; set; } = new Dictionary<string, object?>();
    }
}