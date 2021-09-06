using System;

namespace Elsa.Contracts
{
    public interface IActivityDriverActivator
    {
        IActivityDriver ActivateDriver(Type driverType);
        IActivityDriver? ActivateDriver(IActivity activity);
        IActivityDriver? ActivateDriver(string activityType);
    }
}