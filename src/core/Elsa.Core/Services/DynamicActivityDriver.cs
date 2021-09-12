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
            var activity = (DynamicActivity)context.Node.Activity;
            return ExecuteAsync(activity, context);
        }

        protected virtual ValueTask ExecuteAsync(DynamicActivity dynamicActivity, ActivityExecutionContext context)
        {
            Execute(dynamicActivity, context);
            return new ValueTask();
        }

        protected virtual void Execute(DynamicActivity dynamicActivity, ActivityExecutionContext context)
        {
        }
    }
}