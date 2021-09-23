using System.Collections.Generic;
using System.Linq;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.ControlFlow
{
    public class Fork : Activity
    {
        [Outbound] public ICollection<IActivity> Branches { get; set; } = new List<IActivity>();

        protected override void Execute(ActivityExecutionContext context) => context.ScheduleActivities(Branches.Reverse());
    }
}