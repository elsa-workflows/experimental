namespace Elsa.Persistence.Abstractions.Models
{
    public class WorkflowTrigger
    {
        public string Id { get; set; } = default!;
        public string WorkflowId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Hash { get; set; }
    }
}