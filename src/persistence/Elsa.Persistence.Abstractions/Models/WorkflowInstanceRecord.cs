using Elsa.State;

namespace Elsa.Persistence.Abstractions.Models
{
    public record WorkflowInstanceRecord
    {
        public string Id { get; set; } = default!;
        public string DefinitionId { get; set; } = default!;
        public int Version { get; set; }
        public WorkflowState WorkflowState { get; set; } = default!;
    }
}