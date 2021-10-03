using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowRegistry
    {
        Task<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PagedList<WorkflowDefinition>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken);
    }
}