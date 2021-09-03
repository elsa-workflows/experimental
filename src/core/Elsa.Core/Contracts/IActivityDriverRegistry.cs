using System;

namespace Elsa.Contracts
{
    public interface IActivityDriverRegistry
    {
        void Register(string activityType, Type driverType);
        Type? GetDriverType(IActivity activity);
        Type? GetDriverType(string activityType);
    }
}