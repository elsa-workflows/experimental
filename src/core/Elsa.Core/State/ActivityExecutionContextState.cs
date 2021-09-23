using System.Collections.Generic;

namespace Elsa.State
{
    public class ActivityExecutionContextState
    {
        public ActivityExecutionContextState()
        {
        }
        
        public string ScheduledActivityId { get; set; } = default!;
        public string? ExecuteDelegateMethodName { get; set; }
        public IDictionary<string, object?> Properties { get; set; } = new Dictionary<string, object?>();
    }
}