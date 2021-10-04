using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityTypeRegistry : IRegistry<ActivityType>
    {
        void Register(ActivityType value);
    }
}