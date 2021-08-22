using System.Collections.Generic;
using System.Linq;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.ControlFlow
{
    public class Fork : CodeActivity
    {
        [Ports] public ICollection<IActivity> Branches { get; set; } = new List<IActivity>();
    }

    public class ForkDriver : ActivityDriver<Fork>
    {
        protected override void Execute(Fork activity, ActivityExecutionContext context) => context.ScheduleActivities(activity.Branches.Reverse());
    }
}