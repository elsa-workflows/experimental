using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Runtime.Contracts
{
    /// <summary>
    /// Represents a source of workflows.
    /// </summary>
    public interface IWorkflowProvider
    {
        ValueTask<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<IEnumerable<WorkflowDefinition>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        ValueTask<PagedList<WorkflowDefinition>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken = default);
    }
}