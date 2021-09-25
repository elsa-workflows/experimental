using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.ControlFlow
{
    public enum JoinMode
    {
        WaitAny,
        WaitAll
    }

    public class Join : Activity
    {
        [Input] public Input<JoinMode> JoinMode { get; set; } = new(ControlFlow.JoinMode.WaitAny);
        [Outbound] public IActivity? Next { get; set; }

        protected override void Execute(ActivityExecutionContext context)
        {
            if (Next == null)
                return;

            context.ScheduleActivity(Next);
        }
    }
}