using System.Collections.Generic;
using Elsa.State;

namespace Elsa.Models
{
    public record ExecuteWorkflowResult(WorkflowState WorkflowState, IReadOnlyCollection<Bookmark> Bookmarks);
}