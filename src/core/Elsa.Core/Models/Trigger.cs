using System.Collections.Generic;

namespace Elsa.Models
{
    public record Trigger(
        string Name,
        string? Hash,
        string ActivityId,
        IDictionary<string, object?> Data
    );
}