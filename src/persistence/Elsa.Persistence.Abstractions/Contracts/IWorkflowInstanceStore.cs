using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Entities;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Persistence.Abstractions.Contracts;

public interface IWorkflowInstanceStore
{
    Task<WorkflowInstance?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task SaveAsync(WorkflowInstance record, CancellationToken cancellationToken = default);
    Task SaveManyAsync(IEnumerable<WorkflowInstance> records, CancellationToken cancellationToken = default);
}