using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public record Bookmark(ScheduledNode Target, string Name, IDictionary<string, object?> Data, ExecuteNodeDelegate? Resume);
}