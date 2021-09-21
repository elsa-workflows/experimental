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
        
        protected override void Execute(ActivityExecutionContext context)
        {
            var result = context.Get(Condition);
            var nextNode = result ? Then : Else;

            if (nextNode != null)
                context.ScheduleActivity(nextNode);
        }
    }
}