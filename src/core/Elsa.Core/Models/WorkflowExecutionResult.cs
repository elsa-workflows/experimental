using System.Collections.Generic;
using Elsa.State;

namespace Elsa.Models
{
    public record WorkflowExecutionResult(WorkflowState WorkflowState, IReadOnlyCollection<Bookmark> Bookmarks);
}