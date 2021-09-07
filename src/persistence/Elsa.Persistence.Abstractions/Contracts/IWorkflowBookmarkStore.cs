using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Persistence.Abstractions.Contracts
{
    public interface IWorkflowBookmarkStore
    {
        Task SaveAsync(WorkflowBookmark record, CancellationToken cancellationToken = default);
        Task SaveManyAsync(IEnumerable<WorkflowBookmark> records, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowBookmark>> FindManyAsync(string name, string? hash, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}