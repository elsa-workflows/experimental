using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityScheduler
    {
        bool HasAny { get; }
        void Schedule(ScheduledActivity activity);
        ScheduledActivity Unschedule();
        IEnumerable<ScheduledActivity> List();
    }
}