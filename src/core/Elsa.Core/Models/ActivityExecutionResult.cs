using System.Collections.Generic;
using Elsa.State;

namespace Elsa.Models
{
    public record ActivityExecutionResult(WorkflowState WorkflowState, IReadOnlyCollection<Bookmark> Bookmarks);
}