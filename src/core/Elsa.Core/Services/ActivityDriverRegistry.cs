using System;
using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Services
{
    public class ActivityDriverRegistry : IActivityDriverRegistry
    {
        public ActivityDriverRegistry(IOptions<WorkflowEngineOptions> options)
        {
            Dictionary = new Dictionary<string, Type>(options.Value.Drivers);
        }

        private IDictionary<string, Type> Dictionary { get; }
        public void Register(string activityType, Type driverType) => Dictionary.Add(activityType, driverType);
        public Type? GetDriverType(IActivity activity) => GetDriverType(activity.ActivityType);
        public Type? GetDriverType(string activityType) => Dictionary.TryGetValue(activityType, out var driverType) ? driverType : null;
    }
}