using System.Collections.Generic;

namespace Elsa.Persistence.Abstractions.Models
{
    public class BookmarkRecord
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Hash { get; set; }
        
        public string WorkflowDefinitionId { get; set; } = default!;
        public string? WorkflowInstanceId { get; set; }
        
        public string ActivityId { get; set; } = default!;
        public IDictionary<string, object?> Data { get; set; } = new Dictionary<string, object?>();
        public string? CallbackMethodName { get; set; }
    }
}