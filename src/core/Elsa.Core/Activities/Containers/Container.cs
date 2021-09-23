using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers
{
    public abstract class Container : Activity, IContainer
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
                var value = await evaluator.EvaluateAsync(variable.DefaultValue!, context.ExpressionExecutionContext);
                variable.Set(context, value);
            }

            // Schedule children.
            ScheduleChildren(context);
        }

        public virtual async ValueTask CompleteChildAsync(ActivityExecutionContext context, ActivityExecutionContext childContext) => await OnChildCompleteAsync(context, childContext);

        protected abstract void ScheduleChildren(ActivityExecutionContext context);

        protected virtual ValueTask OnChildCompleteAsync(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
            OnChildComplete(context, childContext);
            return ValueTask.CompletedTask;
        }

        protected virtual void OnChildComplete(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
        }
    }
}