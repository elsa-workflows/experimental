namespace Elsa.Models
{
    public class WorkflowTrigger
    {
        public string Id { get; set; } = default!;
        public string WorkflowDefinitionId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Hash { get; set; }
    }
}