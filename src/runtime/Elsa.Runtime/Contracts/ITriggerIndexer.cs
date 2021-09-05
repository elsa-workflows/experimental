using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface ITriggerIndexer
    {
        Task<IEnumerable<WorkflowTrigger>> IndexTriggersAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
    }
}