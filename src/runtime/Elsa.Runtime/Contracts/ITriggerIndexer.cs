using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface ITriggerIndexer
    {
        Task IndexTriggersAsync(Workflow workflow, CancellationToken cancellationToken = default);
    }
}