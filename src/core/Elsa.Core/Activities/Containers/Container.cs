using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers
{
    public abstract class Container<TActivity> : Activity, IContainer where TActivity : IActivity
    {
        public Container()
        {
        }

        public Container(params IActivity[] activities)
        {
            Activities = activities;
        }

        [Outbound] public ICollection<IActivity> Activities { get; set; } = new List<IActivity>();
        public ICollection<Variable> Variables { get; set; } = new Collection<Variable>();

        public override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            // Register variables.
            context.Register.Declare(Variables);

            // Initialize variables.
            var evaluator = context.WorkflowExecutionContext.GetRequiredService<IExpressionEvaluator>();
            var variablesWithDefaultValues = Variables.Where(x => x.DefaultValue != null);

            foreach (var variable in variablesWithDefaultValues)
            {
                var value = await evaluator.EvaluateAsync(variable.DefaultValue!, new ExpressionExecutionContext(context));
                variable.Set(context, value);
            }

            // Schedule children.
            ScheduleChildren(context);
        }

        public virtual async ValueTask CompleteChildAsync(ActivityExecutionContext childContext, IActivity owner) => await OnChildCompleteAsync(childContext, (TActivity)owner);

        protected abstract void ScheduleChildren(ActivityExecutionContext context);

        protected virtual ValueTask OnChildCompleteAsync(ActivityExecutionContext childContext, TActivity owner)
        {
            OnChildComplete(childContext, owner);
            return ValueTask.CompletedTask;
        }

        protected virtual void OnChildComplete(ActivityExecutionContext childContext, TActivity owner)
        {
        }
    }
}