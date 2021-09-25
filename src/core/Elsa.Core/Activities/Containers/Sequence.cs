using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers
{
    public class Sequence : Container
    {
        private const string CurrentIndexProperty = "CurrentIndex";
        
        public Sequence()
        {
        }

        public Sequence(params IActivity[] activities) : base(activities)
        {
        }

        protected override void ScheduleChildren(ActivityExecutionContext context)
        {
            HandleItem(context);
        }

        private void HandleItem(ActivityExecutionContext context)
        {
            var currentIndex = context.GetProperty<int>(CurrentIndexProperty);
            var childActivities = Activities.ToList();
            
            if (currentIndex >= childActivities.Count)
                return;
            
            var nextActivity = childActivities.ElementAt(currentIndex);
            context.ScheduleActivity(nextActivity, OnChildCompleted);
            context.UpdateProperty<int>(CurrentIndexProperty, x => x + 1);
        }

        private ValueTask OnChildCompleted(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
            HandleItem(context);
            return ValueTask.CompletedTask;
        }
    }
}