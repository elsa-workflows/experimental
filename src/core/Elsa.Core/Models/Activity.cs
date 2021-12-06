using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Helpers;

namespace Elsa.Models;

public abstract class Activity : IActivity
{
    protected Activity() => ActivityType = TypeNameHelper.GenerateActivityTypeName(GetType());
    protected Activity(string activityType) => ActivityType = activityType;

    public string Id { get; set; } = default!;
    public string ActivityType { get; set; }
    public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

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