using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class TriggerTypeRegistry : ITriggerTypeRegistry
    {
        private readonly IDictionary<string, TriggerType> _dictionary = new Dictionary<string, TriggerType>();
        public void Register(TriggerType value) => Register(value.TypeName, value);
        public void Register(string key, TriggerType value) => _dictionary.Add(key, value);
        public IEnumerable<TriggerType> List() => _dictionary.Values.ToList();
        public TriggerType Get(string typeName) => _dictionary.TryGetValue(typeName, out var triggerType) ? triggerType : throw new Exception($"Could not find trigger with type name {typeName}.");
    }
}