using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeInvoker
    {
        Task InvokeAsync(INode node, CancellationToken cancellationToken = default);
    }
}