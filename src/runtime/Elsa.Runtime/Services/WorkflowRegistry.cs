using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Extensions;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Services
{
    public class WorkflowRegistry : IWorkflowRegistry
    {
        private readonly IEnumerable<IWorkflowProvider> _workflowProviders;

        public WorkflowRegistry(IEnumerable<IWorkflowProvider> workflowProviders) => _workflowProviders = workflowProviders;

        public async Task<Workflow?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            foreach (var workflowProvider in _workflowProviders)
            {
                var workflow = await workflowProvider.GetByIdAsync(id, cancellationToken);

                if (workflow != null)
                    return workflow;
            }

            return default!;
        }
        
        public async Task<IEnumerable<Workflow>> GetManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default) =>
            await GetManyByIdInternalAsync(ids, cancellationToken).ToListAsync(cancellationToken);

        public async Task<PagedList<Workflow>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken)
        {
            var tasks = _workflowProviders.Select(x => x.ListAsync(pagerParameters, cancellationToken).AsTask());
            var workflows = (await Task.WhenAll(tasks)).SelectMany(x => x.Items);
            return workflows.Paginate(pagerParameters);
        }

        private async IAsyncEnumerable<Workflow> GetManyByIdInternalAsync(IEnumerable<string> ids, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var idList = ids as ICollection<string> ?? ids.ToHashSet();

            foreach (var workflowProvider in _workflowProviders)
            {
                var workflowDefinitions = await workflowProvider.FindManyByIdAsync(idList, cancellationToken);

                foreach (var workflowDefinition in workflowDefinitions)
                    yield return workflowDefinition;
            }
        }
    }
}