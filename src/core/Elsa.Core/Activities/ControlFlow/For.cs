using System;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.ControlFlow
{
    public enum ForOperator
    {
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }

    public class For : CodeActivity
    {
        [Input] public int Start { get; set; } = 0;
        [Input] public int End { get; set; }
        [Input] public int Step { get; set; } = 1;
        [Input] public ForOperator Operator { get; set; } = ForOperator.LessThanOrEqual;
        [Port] public IActivity? Iterate { get; set; }
        [Port] public IActivity? Next { get; set; }
        public int? CurrentValue { get; set; }
    }

    public class ForDriver : ActivityDriver<For>
    {
        protected override void Execute(For activity, ActivityExecutionContext context)
        {
            var iterateNode = activity.Iterate;

            if (iterateNode == null)
                return;

            HandleIteration(context, activity);
        }

        private ValueTask OnChildComplete(ActivityExecutionContext childContext, IActivity owner)
        {
            var activity = (For)owner;
            HandleIteration(childContext, activity);
            return new ValueTask();
        }

        private void HandleIteration(ActivityExecutionContext context, For activity)
        {
            var iterateNode = activity.Iterate!;
            var end = activity.End;
            var currentValue = activity.CurrentValue != null ? activity.CurrentValue + activity.Step : activity.Start;

            var loop = activity.Operator switch
            {
                ForOperator.LessThan => currentValue < end,
                ForOperator.LessThanOrEqual => currentValue <= end,
                ForOperator.GreaterThan => currentValue > end,
                ForOperator.GreaterThanOrEqual => currentValue >= end,
                _ => throw new NotSupportedException()
            };

            if (loop)
            {
                activity.CurrentValue = currentValue;
                context.WorkflowExecutionContext.Schedule(iterateNode, activity, OnChildComplete);
                return;
            }

            activity.CurrentValue = null;

            if (activity.Next != null)
                context.ScheduleActivity(activity.Next);
        }
    }
}