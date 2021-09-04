using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Persistence.Abstractions.Contracts
{
    public interface IWorkflowInstanceStore
    {
        Task<WorkflowInstanceRecord?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task SaveAsync(WorkflowInstanceRecord record, CancellationToken cancellationToken = default);
    }
}