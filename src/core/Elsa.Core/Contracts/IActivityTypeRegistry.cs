using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityTypeRegistry
    {
        void Register(ActivityType activityType);
        IEnumerable<ActivityType> List();
    }
}