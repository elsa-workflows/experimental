using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Runtime.Contracts
{
    public class WorkflowStore : IWorkflowStore
    {
        private readonly IEnumerable<IWorkflowProvider> _workflowProviders;

        public WorkflowStore(IEnumerable<IWorkflowProvider> workflowProviders)
        {
            _workflowProviders = workflowProviders;
        }

        public async Task<IActivity?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            foreach (var workflowProvider in _workflowProviders)
            {
                var workflow = await workflowProvider.FindByIdAsync(id, cancellationToken);

                if (workflow != null)
                    return workflow;
            }

            return default!;
        }
    }
}