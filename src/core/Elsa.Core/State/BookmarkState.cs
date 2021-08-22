using System.Collections.Generic;

namespace Elsa.State
{
    public record BookmarkState(ScheduledActivityState ScheduledActivity, string Name, IDictionary<string, object?> Data, string? ResumeActionName);
}