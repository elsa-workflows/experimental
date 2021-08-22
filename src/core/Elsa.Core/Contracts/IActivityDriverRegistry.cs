using System;

namespace Elsa.Contracts
{
    public interface IActivityDriverRegistry
    {
        void Register(string activityType, Type driverType);
        IActivityDriver? GetDriver(IActivity activity);
        IActivityDriver? GetDriver(string activityType);
    }
}