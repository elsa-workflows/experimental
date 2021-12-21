using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Elsa.Persistence.EntityFrameworkCore.Contracts;
using Elsa.Persistence.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Persistence.EntityFrameworkCore.Services;

public class EFCoreStore<TEntity> : IStore<TEntity> where TEntity : class
{
    private readonly IDbContextFactory<ElsaDbContext> _dbContextFactory;
    private readonly IServiceProvider _serviceProvider;
    //private readonly SemaphoreSlim _semaphore = new(1);

    public EFCoreStore(IDbContextFactory<ElsaDbContext> dbContextFactory, IServiceProvider serviceProvider)
    {
        _dbContextFactory = dbContextFactory;
        _serviceProvider = serviceProvider;
    }

    public async Task SaveAsync(string id, TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        OnSaving(dbContext, entity);
        await dbContext.BulkInsertOrUpdateAsync(new[] { entity }, config => { config.EnableShadowProperties = true; }, cancellationToken: cancellationToken);
        
        // await _semaphore.WaitAsync(cancellationToken);
        //
        // try
        // {
        //     await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        //     var set = dbContext.Set<TEntity>();
        //     var existingEntity = await set.FindAsync(new object[] { id }, cancellationToken);
        //
        //     OnSaving(dbContext, entity);
        //
        //     if (existingEntity == null)
        //         await set.AddAsync(entity, cancellationToken);
        //     else
        //         set.Attach(entity).State = EntityState.Modified;
        //
        //     await dbContext.SaveChangesAsync(cancellationToken);
        // }
        // finally
        // {
        //     _semaphore.Release();
        // }
    }

    public async Task SaveManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entityList = entities.ToList();
        OnSaving(dbContext, entityList);
        await dbContext.BulkInsertOrUpdateAsync(entityList, config => { config.EnableShadowProperties = true; }, cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var set = dbContext.Set<TEntity>();
        var entity = await set.FirstOrDefaultAsync(predicate, cancellationToken);
        return OnLoading(dbContext, entity);
    }

    public async Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var set = dbContext.Set<TEntity>();
        var entities = await set.Where(predicate).ToListAsync(cancellationToken);
        return OnLoading(dbContext, entities);
    }

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var set = dbContext.Set<TEntity>();
        set.Attach(entity).State = EntityState.Deleted;
        return await dbContext.SaveChangesAsync(cancellationToken) == 1;
    }

    public async Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var set = dbContext.Set<TEntity>();
        return await set.Where(predicate).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
    }

    private void OnSaving(ElsaDbContext dbContext, TEntity entity)
    {
        var handler = _serviceProvider.GetService<IEntitySerializer<TEntity>>();
        handler?.Serialize(dbContext, entity);
    }

    private void OnSaving(ElsaDbContext dbContext, IEnumerable<TEntity> entities)
    {
        var handler = _serviceProvider.GetService<IEntitySerializer<TEntity>>();

        if (handler == null)
            return;

        foreach (var entity in entities)
            handler.Serialize(dbContext, entity);
    }

    private TEntity? OnLoading(ElsaDbContext dbContext, TEntity? entity)
    {
        if (entity == null)
            return null;
        
        var handler = _serviceProvider.GetService<IEntitySerializer<TEntity>>();
        handler?.Deserialize(dbContext, entity);
        return entity;
    }
    
    private IEnumerable<TEntity> OnLoading(ElsaDbContext dbContext, ICollection<TEntity> entities)
    {
        var handler = _serviceProvider.GetService<IEntitySerializer<TEntity>>();

        if (handler == null)
            return entities;

        foreach (var entity in entities)
            handler.Deserialize(dbContext, entity);

        return entities;
    }
}