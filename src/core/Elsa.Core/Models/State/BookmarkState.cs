using System.Collections.Generic;

namespace Elsa.Models.State
{
    public record BookmarkState(string TargetNodeId, string Name, IDictionary<string, object?> Data, string? ResumeAction);
}