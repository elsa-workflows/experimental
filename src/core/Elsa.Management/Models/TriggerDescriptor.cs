using System.Text.Json;
using System.Text.Json.Serialization;
using Elsa.Contracts;

namespace Elsa.Management.Models;

public class TriggerDescriptor
{
    public string TriggerType { get; init; } = default!;
    public string Category { get; init; } = default!;
    [JsonIgnore] public Func<TriggerConstructorContext, ITrigger> Constructor { get; init; } = default!;
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public ICollection<InputDescriptor> InputProperties { get; init; } = new List<InputDescriptor>();
}

public record TriggerConstructorContext(JsonElement Element, JsonSerializerOptions SerializerOptions);