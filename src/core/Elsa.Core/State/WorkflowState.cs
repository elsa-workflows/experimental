using System.Collections.Generic;

namespace Elsa.State
{
    public class WorkflowState
    {
        public IDictionary<string, IDictionary<string, object?>> ActivityOutput { get; set; } = new Dictionary<string, IDictionary<string, object?>>();
    }
}