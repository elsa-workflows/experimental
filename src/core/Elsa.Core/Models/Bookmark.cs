using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public record Bookmark(ScheduledActivity Target, string Name, IDictionary<string, object?> Data, ExecuteActivityDelegate? Resume);
}