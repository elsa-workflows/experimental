using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeInvoker
    {
        Task<WorkflowExecutionContext> InvokeAsync(INode node, CancellationToken cancellationToken = default);
    }
}