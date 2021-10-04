using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityTypeRegistry : IActivityTypeRegistry
    {
        private readonly IDictionary<string, ActivityType> _dictionary = new Dictionary<string, ActivityType>();
        public void Register(ActivityType value) => _dictionary.Add(value.TypeName, value);
        public void Register(string key, ActivityType value) => _dictionary.Add(key, value);

        public IEnumerable<ActivityType> List() => _dictionary.Values.ToList();
        public ActivityType Get(string typeName) => _dictionary.TryGetValue(typeName, out var triggerType) ? triggerType : throw new Exception($"Could not find trigger with type name {typeName}.");
    }
}