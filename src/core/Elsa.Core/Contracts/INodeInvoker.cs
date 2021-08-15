using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Contracts
{
    public interface INodeInvoker
    {
        Task InvokeAsync(INode node, CancellationToken cancellationToken = default);
    }
}