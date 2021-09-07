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
    }

    public class SequenceDriver : ContainerActivityDriver<Sequence>
    {
        protected override void Execute(Sequence activity, ActivityExecutionContext context)
        {
            var childActivities = activity.Activities.ToList();
            var firstActivity = childActivities.FirstOrDefault();

            if (firstActivity != null)
                context.ScheduleActivity(firstActivity);
        }

        protected override void OnChildComplete(ActivityExecutionContext childContext, Sequence owner)
        {
            var sequence = owner;
            var childActivities = sequence.Activities.ToList();
            var completedActivity = childContext.Activity;
            var nextIndex = childActivities.IndexOf(completedActivity) + 1;

            if (nextIndex >= childActivities.Count) 
                return;
            
            var nextActivity = childActivities.ElementAt(nextIndex);
            childContext.ScheduleActivity(nextActivity);
        }
    }
}