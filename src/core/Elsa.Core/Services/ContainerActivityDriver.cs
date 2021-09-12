using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public abstract class ContainerActivityDriver<TActivity> : ActivityDriver<TActivity>, IContainerDriver where TActivity : IActivity, new()
    {
        ValueTask IContainerDriver.OnChildCompleteAsync(ActivityExecutionContext childContext, IActivity owner)
        {
            var activity = (TActivity)owner;
            return OnChildCompleteAsync(childContext, activity);
        }

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