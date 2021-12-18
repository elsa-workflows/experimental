using System.Linq.Expressions;
using Elsa.Persistence.EntityFrameworkCore.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Persistence.EntityFrameworkCore.Services;

public class EFCoreStore<TEntity> : IStore<TEntity> where TEntity : class
{
    private readonly IDbContextFactory<ElsaDbContext> _dbContextFactory;
    private readonly SemaphoreSlim _semaphore = new(1);

    public EFCoreStore(IDbContextFactory<ElsaDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task SaveAsync(string id, TEntity entity, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var set = dbContext.Set<TEntity>();
            var existingEntity = await set.FindAsync(new object[] { id }, cancellationToken);

            if (existingEntity == null)
                await set.AddAsync(entity, cancellationToken);
            else
                set.Attach(entity).State = EntityState.Modified;

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Task SaveManyAsync(TEntity entity, Func<TEntity, string> idAccessor, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}