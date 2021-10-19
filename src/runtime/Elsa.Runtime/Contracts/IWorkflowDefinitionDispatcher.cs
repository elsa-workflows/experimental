using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowDefinitionDispatcher
    {
        Task DispatchAsync(DispatchWorkflowDefinitionRequest request, CancellationToken cancellationToken = default);
    }
}