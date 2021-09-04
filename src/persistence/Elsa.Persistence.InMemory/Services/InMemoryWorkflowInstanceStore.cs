using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Persistence.InMemory.Services
{
    public class InMemoryWorkflowInstanceStore : IWorkflowInstanceStore
    {
        private readonly ConcurrentDictionary<string, WorkflowInstanceRecord> _records = new();

        public Task<WorkflowInstanceRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var record = _records.TryGetValue(id, out var result) ? result : default;
            return Task.FromResult(record);
        }

        public Task SaveAsync(WorkflowInstanceRecord record, CancellationToken cancellationToken = default)
        {
            _records.AddOrUpdate(record.Id, record, (_, _) => record);
            return Task.CompletedTask;
        }
    }
}