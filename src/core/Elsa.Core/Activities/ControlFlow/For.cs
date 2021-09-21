using System;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

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

        protected override void Execute(ActivityExecutionContext context)
        {
            var iterateNode = Iterate;

            if (iterateNode == null)
                return;

            context.Register.Declare(CurrentValue);
            HandleIteration(context);
        }
        
        private void HandleIteration(ActivityExecutionContext context)
        {
            var iterateNode = Iterate!;
            var end = End;
            var currentValue = CurrentValue.Get<int?>(context);

            // Initialize or increment.
            currentValue = currentValue == null ? Start : currentValue + Step;

            var loop = Operator switch
            {
                ForOperator.LessThan => currentValue < end,
                ForOperator.LessThanOrEqual => currentValue <= end,
                ForOperator.GreaterThan => currentValue > end,
                ForOperator.GreaterThanOrEqual => currentValue >= end,
                _ => throw new NotSupportedException()
            };

            if (loop)
            {
                context.ScheduleActivity(iterateNode, this, OnChildComplete);

                // Update loop variable.
                CurrentValue.Set(context, currentValue);
                return;
            }

            if (Next != null)
                context.ScheduleActivity(Next);
        }

        private ValueTask OnChildComplete(ActivityExecutionContext completedActivityExecutionContext, ActivityExecutionContext ownerActivityExecutionContext)
        {
            HandleIteration(ownerActivityExecutionContext);
            return ValueTask.CompletedTask;
        }
    }
}