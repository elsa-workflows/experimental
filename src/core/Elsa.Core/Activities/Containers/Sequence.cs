using System.Linq;
using System.Threading.Tasks;
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

    public class SequenceDriver : ActivityDriver<Sequence>
    {
        protected override void Execute(Sequence activity, ActivityExecutionContext context)
        {
            var childActivities = activity.Activities.ToList();
            var firstActivity = childActivities.FirstOrDefault();

            if (firstActivity != null)
                context.ScheduleActivity(firstActivity, OnChildComplete);
        }

        private ValueTask OnChildComplete(ActivityExecutionContext childContext, IActivity owner)
        {
            var sequence = (Sequence)owner;
            var childActivities = sequence.Activities.ToList();
            var completedActivity = childContext.Activity;
            var nextIndex = childActivities.IndexOf(completedActivity) + 1;

            if (nextIndex < childActivities.Count)
            {
                var nextActivity = childActivities.ElementAt(nextIndex);
                childContext.WorkflowExecutionContext.Schedule(nextActivity, owner, OnChildComplete);
            }

            return new ValueTask();
        }
    }
}