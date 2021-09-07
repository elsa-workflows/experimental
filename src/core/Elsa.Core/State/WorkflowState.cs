using System.Collections.Generic;

namespace Elsa.State
{
    public class WorkflowState
    {
        public string WorkflowInstanceId { get; set; } = default!;
        public IDictionary<string, IDictionary<string, object?>> ActivityOutput { get; set; } = new Dictionary<string, IDictionary<string, object?>>();
        public IDictionary<string, string> CompletionCallbacks { get; set; } = new Dictionary<string, string>();
    }
}