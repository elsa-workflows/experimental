using System.Collections.Generic;

namespace Elsa.Runtime.Contracts
{
    public class WorkflowTrigger
    {
        public string WorkflowDefinitionId { get; set; } = default!;
        public string ActivityTypeName { get; set; } = default!;
        public string ActivityId { get; set; } = default!;
        public string? Hash { get; set; }
        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}