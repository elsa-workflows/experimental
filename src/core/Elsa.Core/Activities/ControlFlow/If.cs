using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.ControlFlow
{
    public class If : Activity
    {
        [Input] public Input<bool> Condition { get; set; } = new(new Literal<bool>(false));
        [Port] public IActivity? Then { get; set; }
        [Port] public IActivity? Else { get; set; }
    }

    public class IfDriver : ActivityDriver<If>
    {
        protected override void Execute(If activity, ActivityExecutionContext context)
        {
            var result = context.Get(activity.Condition);
            var nextNode = result ? activity.Then : activity.Else;

            if (nextNode != null)
                context.ScheduleActivity(nextNode);
        }
    }
}