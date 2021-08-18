using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INotifyNodeExecuted
    {
        ValueTask OnChildComplete(NodeExecutionContext childContext, INode owner);
    }
}