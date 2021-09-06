﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Persistence.InMemory.Services
{
    public class InMemoryWorkflowBookmarkStore : IWorkflowBookmarkStore
    {
        private readonly ConcurrentDictionary<string, WorkflowBookmark> _records = new();

        public Task SaveAsync(WorkflowBookmark record, CancellationToken cancellationToken = default)
        {
            _records.AddOrUpdate(record.Id, record, (_, _) => record);
            return Task.CompletedTask;
        }

        public async Task SaveManyAsync(IEnumerable<WorkflowBookmark> records, CancellationToken cancellationToken = default)
        {
            foreach (var record in records) 
                await SaveAsync(record, cancellationToken);
        }

        public Task<IEnumerable<WorkflowBookmark>> FindManyAsync(string name, string? hash, CancellationToken cancellationToken = default)
        {
            var results = _records.Values.Where(x => x.Name == name && x.Hash == hash);
            return Task.FromResult(results);
        }

        public Task DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
                _records.TryRemove(id, out _);

            return Task.CompletedTask;
        }
    }
}