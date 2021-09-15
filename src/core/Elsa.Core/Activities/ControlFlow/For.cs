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

    public class For : Activity
    {
        [Input] public int Start { get; set; } = 0;
        [Input] public int End { get; set; }
        [Input] public int Step { get; set; } = 1;
        [Input] public ForOperator Operator { get; set; } = ForOperator.LessThanOrEqual;
        [Port] public IActivity? Iterate { get; set; }
        [Port] public IActivity? Next { get; set; }
        public Variable<int?> CurrentValue { get; set; } = new();
    }

    public class ForDriver : ActivityDriver<For>
    {
        protected override void Execute(For activity, ActivityExecutionContext context)
        {
            var iterateNode = activity.Iterate;

            if (iterateNode == null)
                return;

            context.Register.Declare(activity.CurrentValue);
            HandleIteration(context, activity);
        }

        private void HandleIteration(ActivityExecutionContext context, For activity)
        {
            var iterateNode = activity.Iterate!;
            var end = activity.End;
            var currentValue = activity.CurrentValue.Get<int?>(context);

            // Initialize or increment.
            currentValue = currentValue == null ? activity.Start : currentValue + activity.Step;

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
                context.ScheduleActivity(iterateNode, activity, OnChildComplete);

                // Update loop variable.
                activity.CurrentValue.Set(context, currentValue);
                return;
            }

            if (activity.Next != null)
                context.ScheduleActivity(activity.Next);
        }

        private ValueTask OnChildComplete(ActivityExecutionContext completedActivityExecutionContext, ActivityExecutionContext ownerActivityExecutionContext)
        {
            var activity = (For)ownerActivityExecutionContext.Activity;
            HandleIteration(ownerActivityExecutionContext, activity);
            return ValueTask.CompletedTask;
        }
    }
}