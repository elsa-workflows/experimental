using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowManager
    {
        Task<Workflow?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PagedList<Workflow>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken);
        Task<WorkflowExecutionResult> ExecuteWorkflowAsync(Workflow workflow, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowExecutionResult>> ResumeWorkflowsAsync(string bookmarkName, string? hash, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowExecutionResult>> TriggerWorkflowsAsync(string triggerName, string? hash, CancellationToken cancellationToken = default);
    }
}