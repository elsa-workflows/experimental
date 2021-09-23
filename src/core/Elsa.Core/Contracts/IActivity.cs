using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivity : INode
    {
        string ActivityId { get; set; }
        string ActivityType { get; }
        ValueTask ExecuteAsync(ActivityExecutionContext context);
    }
}