using System.Collections.Generic;

namespace Elsa.Persistence.Entities;

public class WorkflowBookmark : Entity
{
    public string Name { get; set; } = default!;
    public string? Hash { get; set; }
    public string WorkflowDefinitionId { get; set; } = default!;
    public string WorkflowInstanceId { get; set; } = default!;
    public string CorrelationId { get; set; } = default!;
    public string ActivityId { get; set; } = default!;
    public string ActivityInstanceId { get; set; } = default!;
    public IDictionary<string, object?>? Data { get; set; }
    public string? CallbackMethodName { get; set; }
}