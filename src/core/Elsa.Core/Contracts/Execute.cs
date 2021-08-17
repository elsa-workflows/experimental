using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public delegate ValueTask<INodeExecutionResult> Execute(NodeExecutionContext context);
}