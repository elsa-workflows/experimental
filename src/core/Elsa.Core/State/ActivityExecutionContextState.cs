using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.State
{
    public class ActivityExecutionContextState
    {
        public ActivityExecutionContextState()
        {
        }

        public ActivityExecutionContextState? ParentActivityExecutionContext { get; set; }
        public string ScheduledActivityId { get; set; } = default!;
        public string? ExecuteDelegateMethodName { get; set; }
        public IDictionary<string, object?> Properties { get; set; } = new Dictionary<string, object?>();
    }
}