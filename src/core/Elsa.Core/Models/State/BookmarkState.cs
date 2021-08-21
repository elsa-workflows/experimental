using System.Collections.Generic;

namespace Elsa.Models.State
{
    public record BookmarkState(ScheduledNodeState ScheduledNode, string Name, IDictionary<string, object?> Data, string? ResumeActionName);
}