using System.Collections.Generic;

namespace Elsa.Models
{
    public record Bookmark(
        string Id,
        string Name,
        string? Hash,
        string ActivityId,
        IDictionary<string, object?>? Data = default,
        string? CallbackMethodName = default
    );
}