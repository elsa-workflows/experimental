using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Contracts
{
    public interface IWorkflowInvoker
    {
        Task InvokeAsync(INode root, CancellationToken cancellationToken);
    }
}