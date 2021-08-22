using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public abstract class DynamicActivityDriver : IActivityDriver
    {
        protected abstract string ActivityType { get; }
        public virtual bool GetSupportsActivity(IActivity activity) => activity.ActivityType == ActivityType;
        public virtual int Priority => 0;
        
        ValueTask IActivityDriver.ExecuteAsync(ActivityExecutionContext context)
        {
            var activity = (Activity)context.Node.Activity;
            return ExecuteAsync(activity, context);
        }

        protected virtual ValueTask ExecuteAsync(Activity activity, ActivityExecutionContext context)
        {
            Execute(activity, context);
            return new ValueTask();
        }

        protected virtual void Execute(Activity activity, ActivityExecutionContext context)
        {
        }
    }
}