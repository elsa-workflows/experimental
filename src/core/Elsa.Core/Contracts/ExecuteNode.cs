using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public delegate ValueTask ExecuteNode(NodeExecutionContext context);
}