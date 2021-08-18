using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public delegate ValueTask NodeMiddlewareDelegate(NodeExecutionContext context);

    public delegate ValueTask ExecuteNodeDelegate(NodeExecutionContext context);

    public delegate ValueTask NodeCompletionCallback(NodeExecutionContext context, INode completedNode);
}