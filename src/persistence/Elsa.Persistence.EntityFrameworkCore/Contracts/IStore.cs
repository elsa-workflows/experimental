using System.Linq.Expressions;

namespace Elsa.Persistence.EntityFrameworkCore.Contracts;

public interface IStore<TEntity>
{
    Task SaveAsync(string id, TEntity entity, CancellationToken cancellationToken = default);
    Task SaveManyAsync(TEntity entity, Func<TEntity, string> idAccessor, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(string id, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<int> DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}