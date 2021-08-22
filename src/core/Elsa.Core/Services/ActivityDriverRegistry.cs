using System;
using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elsa.Services
{
    public class ActivityDriverRegistry : IActivityDriverRegistry
    {
        private readonly IServiceProvider _serviceProvider;

        public ActivityDriverRegistry(IOptions<WorkflowEngineOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Dictionary = new Dictionary<string, Type>(options.Value.Drivers);
        }

        private IDictionary<string, Type> Dictionary { get; }
        public void Register(string activityType, Type driverType) => Dictionary.Add(activityType, driverType);
        public IActivityDriver? GetDriver(IActivity activity) => GetDriver(activity.ActivityType);

        public IActivityDriver? GetDriver(string nodeType)
        {
            if (!Dictionary.TryGetValue(nodeType, out var driverType))
                return null;

            return (IActivityDriver)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, driverType);
        }
    }
}