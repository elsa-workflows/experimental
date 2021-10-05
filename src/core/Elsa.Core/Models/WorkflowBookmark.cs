using System.Collections.Generic;

namespace Elsa.Models
{
    public class WorkflowBookmark
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Hash { get; set; }
        public string WorkflowDefinitionId { get; set; } = default!;
        public string WorkflowInstanceId { get; set; } = default!;
        public string ActivityId { get; set; } = default!;
        public string ActivityInstanceId { get; set; } = default!;
        public IDictionary<string, object?>? Data { get; set; }
        public string? CallbackMethodName { get; set; }
    }
}