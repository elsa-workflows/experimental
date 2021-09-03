using System;
using Elsa.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Services
{
    public class ActivityDriverActivator : IActivityDriverActivator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IActivityDriverRegistry _driverRegistry;

        public ActivityDriverActivator(IServiceProvider serviceProvider, IActivityDriverRegistry driverRegistry)
        {
            _serviceProvider = serviceProvider;
            _driverRegistry = driverRegistry;
        }

        public IActivityDriver ActivateDriver(Type driverType) => (IActivityDriver)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, driverType);
        
        public IActivityDriver? GetDriver(IActivity activity)
        {
            var driverType = _driverRegistry.GetDriverType(activity);
            return driverType == null ? null : ActivateDriver(driverType);
        }

        public IActivityDriver? GetDriver(string activityType)
        {
            var driverType = _driverRegistry.GetDriverType(activityType);
            return driverType == null ? null : ActivateDriver(driverType);
        }
    }
}