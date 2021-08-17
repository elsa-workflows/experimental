using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INotifyNodeExecuted
    {
        ValueTask HandleNodeExecuted(NodeExecutionContext childContext, INode owner);
    }
}