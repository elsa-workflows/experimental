using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IContainerDriver
    {
        ValueTask OnChildCompleteAsync(ActivityExecutionContext childContext, IActivity owner);
    }
}