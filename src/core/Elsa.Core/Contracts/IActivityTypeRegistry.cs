using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityTypeRegistry
    {
        void Register(ActivityType activityType);
        IEnumerable<ActivityType> List();
    }
}