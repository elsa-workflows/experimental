using System.Collections.Generic;

namespace Elsa.Models
{
    public record Bookmark(
        string Name,
        string? Hash,
        string ActivityId,
        IDictionary<string, object?> Data,
        string? CallbackMethodName
    );
}