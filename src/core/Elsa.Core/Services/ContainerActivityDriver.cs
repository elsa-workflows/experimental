using System.Linq;
using System.Threading.Tasks;
using Elsa.Activities.Containers;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public abstract class ContainerActivityDriver<TActivity> : ActivityDriver<TActivity>, IContainerDriver where TActivity : Container, new()
    {
        protected override async ValueTask ExecuteAsync(TActivity activity, ActivityExecutionContext context)
        {
            // Register variables.
            context.Register.Declare(activity.Variables);
            
            // Initialize variables.
            var evaluator = context.WorkflowExecutionContext.GetRequiredService<IExpressionEvaluator>();
            var variablesWithDefaultValues = activity.Variables.Where(x => x.DefaultValue != null);
            
            foreach (var variable in variablesWithDefaultValues)
            {
                var value = await evaluator.EvaluateAsync(variable.DefaultValue!, new ExpressionExecutionContext(context));
                variable.Set(context, value);
            }
            
            // Schedule children.
            ScheduleChildren(activity, context);
        }

        ValueTask IContainerDriver.OnChildCompleteAsync(ActivityExecutionContext childContext, IActivity owner)
        {
            var activity = (TActivity)owner;
            return OnChildCompleteAsync(childContext, activity);
        }

        protected abstract void ScheduleChildren(TActivity activity, ActivityExecutionContext context);

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