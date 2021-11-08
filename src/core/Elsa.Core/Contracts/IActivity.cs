using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivity : INode
    {
        string ActivityType { get; set; }
        ValueTask ExecuteAsync(ActivityExecutionContext context);
    }
}