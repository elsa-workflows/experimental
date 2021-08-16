using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Results
{
    public class DoneResult : INodeExecutionResult
    {
        public ValueTask ExecuteAsync(NodeExecutionContext context) => new();
    }
}