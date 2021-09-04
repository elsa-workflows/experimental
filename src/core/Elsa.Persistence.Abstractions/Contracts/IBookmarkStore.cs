using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Persistence.Abstractions.Contracts
{
    public interface IBookmarkStore
    {
        Task SaveAsync(BookmarkRecord record, CancellationToken cancellationToken = default);
        Task SaveManyAsync(IEnumerable<BookmarkRecord> records, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookmarkRecord>> FindManyAsync(string name, string? hash, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}