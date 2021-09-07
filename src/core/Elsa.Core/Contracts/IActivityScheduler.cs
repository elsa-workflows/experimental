using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityScheduler
    {
        bool HasAny { get; }
        void Push(ScheduledActivity activity);
        ScheduledActivity Pop();
        IEnumerable<ScheduledActivity> List();
    }
}