using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IContainer : IActivity
    {
        ValueTask CompleteChildAsync(ActivityExecutionContext childContext, IActivity owner);
    }
}