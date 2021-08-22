using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Contracts
{
    public interface IWorkflowInvoker
    {
        Task InvokeAsync(IActivity root, CancellationToken cancellationToken);
    }
}