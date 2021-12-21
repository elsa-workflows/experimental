using System.Linq.Expressions;

namespace Elsa.Persistence.EntityFrameworkCore.Contracts;

public interface IStore<TEntity>
{
    Task SaveAsync(string id, TEntity entity, CancellationToken cancellationToken = default);
    Task SaveManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}