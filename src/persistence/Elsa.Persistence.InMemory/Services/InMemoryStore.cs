using System;
using System.Collections.Generic;
using System.Linq;

namespace Elsa.Persistence.InMemory.Services;

public class InMemoryStore<TEntity>
{
    private IDictionary<string, TEntity> Entities { get; set; } = new Dictionary<string, TEntity>();
    public void Save(string id, TEntity entity) => Entities[id] = entity;
    public TEntity? Find(Func<TEntity, bool> predicate) => Entities.Values.Where(predicate).FirstOrDefault();
    public IEnumerable<TEntity> FindMany(Func<TEntity, bool> predicate) => Entities.Values.Where(predicate);
}