using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public delegate ValueTask ExecuteNodeMiddlewareDelegate(NodeExecutionContext context);
}