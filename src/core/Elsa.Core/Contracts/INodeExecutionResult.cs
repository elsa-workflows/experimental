using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeExecutionResult
    {
        ValueTask ExecuteAsync(NodeExecutionContext context);
    }
}