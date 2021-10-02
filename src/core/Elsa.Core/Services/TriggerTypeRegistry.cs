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
        public void Register(TriggerType triggerType) => _dictionary.Add(triggerType.TypeName, triggerType);
        public IEnumerable<TriggerType> List() => _dictionary.Values.ToList();
        public TriggerType Get(string typeName) => _dictionary[typeName];
    }
}