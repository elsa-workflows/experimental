using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public abstract class ActivityDriver<TActivity> : IActivityDriver where TActivity : IActivity, new()
    {
        ValueTask IActivityDriver.ExecuteAsync(ActivityExecutionContext context)
        {
            var activity = (TActivity)context.Node.Activity;
            return ExecuteAsync(activity, context);
        }

        protected virtual ValueTask ExecuteAsync(TActivity activity, ActivityExecutionContext context)
        {
            Execute(activity, context);
            return ValueTask.CompletedTask;
        }

        protected virtual void Execute(TActivity activity, ActivityExecutionContext context)
        {
        }
    }

    public abstract class ActivityWithResultDriver<TActivity, TResult> : IActivityDriver where TActivity : ActivityWithResult, new()
    {
        async ValueTask IActivityDriver.ExecuteAsync(ActivityExecutionContext context)
        {
            var activity = (TActivity)context.Node.Activity;
            var result = await ExecuteAsync(activity, context);
            context.Set(activity.Result, result);
        }

        protected virtual ValueTask<TResult> ExecuteAsync(TActivity activity, ActivityExecutionContext context)
        {
            var result = Execute(activity, context);
            return ValueTask.FromResult(result);
        }

        protected virtual TResult Execute(TActivity activity, ActivityExecutionContext context)
        {
            return default!;
        }
    }
}