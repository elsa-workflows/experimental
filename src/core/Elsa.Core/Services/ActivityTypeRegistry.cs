using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityTypeRegistry : IActivityTypeRegistry
    {
        private readonly IDictionary<string, ActivityType> _dictionary = new Dictionary<string, ActivityType>();
        public void Register(ActivityType activityType) => _dictionary.Add(activityType.TypeName, activityType);
        public IEnumerable<ActivityType> List() => _dictionary.Values.ToList();
    }
}