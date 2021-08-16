using System.Collections.Generic;

namespace Elsa.Models
{
    public class WorkflowExecutionContext
    {
        public Stack<ScopedExecutionContext> ScheduledScopes { get; set; } = new();
        public void ScheduleScope(ScopedExecutionContext scope) => ScheduledScopes.Push(scope);
    }
}