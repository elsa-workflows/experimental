using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.ControlFlow
{
    public class ParallelForEach<T> : Activity
    {
        [Input] public Input<ICollection<T>> Items { get; set; } = new(Array.Empty<T>());
        [Outbound] public IActivity Body { get; set; } = default!;
        public Variable<T> CurrentValue { get; set; } = new();

        protected override void Execute(ActivityExecutionContext context)
        {
            // Declare looping variable.
            context.ExpressionExecutionContext.Register.Declare(CurrentValue);

            // Schedule a body of work for each item.
            var items = context.Get(Items)!.Reverse().ToList();

            foreach (var item in items)
            {
                var scheduledActivity = context.ScheduleActivity(Body);
                
                var localVariable = new Variable<T>(item)
                {
                    // "Capture" the CurrentValues variable by use same ID so that outer scope variable can access inner scope variable.
                    Id = CurrentValue.Id
                };

                // Provide the captured variable to the scheduled activity. 
                scheduledActivity.LocationReferences = new[] { localVariable };
            }
        }
    }
}