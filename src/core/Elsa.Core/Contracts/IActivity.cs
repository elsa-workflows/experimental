using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivity : INode
    {
        string ActivityType { get; }
        ValueTask ExecuteAsync(ActivityExecutionContext context);
    }
}