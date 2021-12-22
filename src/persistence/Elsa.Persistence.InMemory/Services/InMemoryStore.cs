using System;
using System.Collections.Generic;
using System.Linq;

namespace Elsa.Persistence.InMemory.Services;

public class InMemoryStore<TEntity>
{
    private IDictionary<string, TEntity> Entities { get; set; } = new Dictionary<string, TEntity>();
    public void Save(string id, TEntity entity) => Entities[id] = entity;

    public void SaveMany(IEnumerable<TEntity> entities, Func<TEntity, string> idAccessor)
    {
        foreach (var entity in entities)
        {
            var id = idAccessor(entity);
            Save(id, entity);
        }
    }

    public TEntity? Find(Func<TEntity, bool> predicate) => Entities.Values.Where(predicate).FirstOrDefault();
    public IEnumerable<TEntity> FindMany(Func<TEntity, bool> predicate) => Entities.Values.Where(predicate);
    public IEnumerable<TEntity> List() => Entities.Values;

    public void Delete(string id) => Entities.Remove(id);

    public int DeleteWhere(Func<TEntity, bool> predicate)
    {
        var query =
            from entry in Entities
            where predicate(entry.Value)
            select entry;

        var entries = query.ToList();
        foreach (var entry in entries)
            Entities.Remove(entry);

        return entries.Count;
    }

    public void DeleteMany(IEnumerable<string> ids)
    {
        foreach (var id in ids)
            Entities.Remove(id);
    }
}