using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstanceDispatcher
    {
        Task DispatchAsync(DispatchWorkflowInstanceRequest request, CancellationToken cancellationToken = default);
    }
}