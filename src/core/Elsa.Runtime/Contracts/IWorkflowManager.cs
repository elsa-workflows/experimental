using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowManager
    {
        Task<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowExecutionResult>> ResumeBookmarksAsync(string bookmarkName, string hash, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> ExecuteWorkflowAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
    }
}