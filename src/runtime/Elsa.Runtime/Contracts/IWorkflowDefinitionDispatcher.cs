using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowDefinitionDispatcher
    {
        Task DispatchAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
    }
}