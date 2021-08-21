using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeScheduler
    {
        bool HasAny { get; }
        void Schedule(ScheduledNode node);
        ScheduledNode Unschedule();
        IEnumerable<ScheduledNode> List();
    }
}