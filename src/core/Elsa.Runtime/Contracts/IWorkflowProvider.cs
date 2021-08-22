using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Runtime.Contracts
{
    /// <summary>
    /// Represents a source of workflows.
    /// </summary>
    public interface IWorkflowProvider
    {
        ValueTask<IActivity?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}