using System;

namespace Elsa.Contracts
{
    public interface IActivityDriverActivator
    {
        IActivityDriver ActivateDriver(Type driverType);
        IActivityDriver? GetDriver(IActivity activity);
        IActivityDriver? GetDriver(string activityType);
    }
}