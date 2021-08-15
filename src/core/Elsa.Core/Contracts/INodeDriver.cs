using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeDriver
    {
        bool GetSupportsNode(INode node);
        int Priority { get; }
        ValueTask<INodeExecutionResult> ExecuteAsync(NodeExecutionContext context);
    }
}