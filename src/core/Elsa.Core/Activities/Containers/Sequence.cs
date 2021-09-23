using System.Linq;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Containers
{
    public class Sequence : Container
    {
        public Sequence()
        {
        }

        public Sequence(params IActivity[] activities) : base(activities)
        {
        }

        protected override void ScheduleChildren(ActivityExecutionContext context)
        {
            // Schedule first child.
            var childActivities = Activities.ToList();
            var firstActivity = childActivities.FirstOrDefault();

            if (firstActivity != null)
                context.ScheduleActivity(firstActivity);
        }
        
        protected override void OnChildComplete(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
            var childActivities = Activities.ToList();
            var completedActivity = childContext.Activity;
            var nextIndex = childActivities.IndexOf(completedActivity) + 1;

            if (nextIndex >= childActivities.Count) 
                return;
            
            var nextActivity = childActivities.ElementAt(nextIndex);
            childContext.ScheduleActivity(nextActivity);
        }
    }
}