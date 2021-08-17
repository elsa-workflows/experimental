using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public delegate ValueTask ExecuteNodeDelegate(NodeExecutionContext context);
}