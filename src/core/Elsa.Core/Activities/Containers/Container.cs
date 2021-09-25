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
            context.ExpressionExecutionContext.Register.Declare(Variables);

            // Initialize variables.
            var evaluator = context.WorkflowExecutionContext.GetRequiredService<IExpressionEvaluator>();
            var variablesWithDefaultValues = Variables.Where(x => x.DefaultValue != null);

            foreach (var variable in variablesWithDefaultValues)
            {
                var expressionExecutionContext = context.ExpressionExecutionContext;
                var value = await evaluator.EvaluateAsync(variable.DefaultValue!, expressionExecutionContext);
                variable.Set(expressionExecutionContext, value);
            }

            // Schedule children.
            ScheduleChildren(context);
        }

        protected abstract void ScheduleChildren(ActivityExecutionContext context);
    }
}