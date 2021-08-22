using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class ActivityScheduler : IActivityScheduler
    {
        private readonly Stack<ScheduledActivity> _stack = new();

        public bool HasAny => _stack.Any();
        public void Schedule(ScheduledActivity activity) => _stack.Push(activity);
        public ScheduledActivity Unschedule() => _stack.Pop();
        public IEnumerable<ScheduledActivity> List() => _stack.ToList();
    }
}