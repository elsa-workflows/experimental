using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public abstract class ActivityDriver<TActivity> : IActivityDriver where TActivity : new()
    {
        ValueTask IActivityDriver.ExecuteAsync(ActivityExecutionContext context)
        {
            var activity = (TActivity)context.Node.Activity;
            return ExecuteAsync(activity, context);
        }

        protected virtual ValueTask ExecuteAsync(TActivity activity, ActivityExecutionContext context)
        {
            Execute(activity, context);
            return new ValueTask();
        }

        protected virtual void Execute(TActivity activity, ActivityExecutionContext context)
        {
        }
    }
}