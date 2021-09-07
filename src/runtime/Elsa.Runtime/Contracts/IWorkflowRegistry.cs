using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowRegistry
    {
        Task<Workflow?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PagedList<Workflow>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken);
    }
}