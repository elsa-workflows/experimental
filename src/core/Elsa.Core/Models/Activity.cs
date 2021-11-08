using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Models
{
    public abstract class Activity : IActivity
    {
        protected Activity() => ActivityType = GetType().Name;
        protected Activity(string activityType) => ActivityType = activityType;

        public string Id { get; set; } = default!;
        public string ActivityType { get; set; }

        public virtual ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            Execute(context);
            return ValueTask.CompletedTask;
        }

        protected virtual void Execute(ActivityExecutionContext context)
        {
        }
    }

    public abstract class ActivityWithResult : Activity
    {
        public Output? Result { get; set; }
    }

    public abstract class Activity<T> : ActivityWithResult
    {
    }
}