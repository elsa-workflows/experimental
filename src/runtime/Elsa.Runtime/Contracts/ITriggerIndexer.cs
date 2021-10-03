using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Runtime.Contracts
{
    public interface ITriggerIndexer
    {
        Task IndexTriggersAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
    }
}