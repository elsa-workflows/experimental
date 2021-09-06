using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    /// <summary>
    /// Represents a source of workflows.
    /// </summary>
    public interface IWorkflowProvider
    {
        ValueTask<Workflow?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<IEnumerable<Workflow>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        ValueTask<PagedList<Workflow>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken = default);
    }
}