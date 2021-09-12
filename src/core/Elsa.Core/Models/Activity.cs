using Elsa.Contracts;

namespace Elsa.Models
{
    public abstract class Activity : IActivity
    {
        public string ActivityId { get; set; } = default!;
        public string ActivityType => GetType().Name;
    }
    
    public abstract class ActivityWithResult : Activity
    {
        public Output? Result { get; set; }
    }

    public abstract class Activity<T> : ActivityWithResult
    {
    }
}