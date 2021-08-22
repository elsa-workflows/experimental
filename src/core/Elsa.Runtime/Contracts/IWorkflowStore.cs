using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowStore
    {
        Task<IActivity?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}