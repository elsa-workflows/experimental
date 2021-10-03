using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Extensions;
using Elsa.Runtime.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Runtime.WorkflowProviders
{
    public class ConfigurationWorkflowProvider : IWorkflowProvider
    {
        private readonly IIdentityGraphService _identityGraphService;
        private readonly WorkflowRuntimeOptions _options;
        private readonly IDictionary<string, WorkflowDefinition> _workflows;
        
        public ConfigurationWorkflowProvider(IOptions<WorkflowRuntimeOptions> options, IIdentityGraphService identityGraphService)
        {
            _identityGraphService = identityGraphService;
            _options = options.Value;
            _workflows = CreateWorkflowDefinitions().ToDictionary(x => x.Id);
        }

        public ValueTask<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = _workflows.TryGetValue(id, out var workflowDefinition) ? workflowDefinition : default;
            return ValueTask.FromResult(result);
        }

        public ValueTask<IEnumerable<WorkflowDefinition>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            var workflowDefinitions = FindManyById(ids);
            return ValueTask.FromResult(workflowDefinitions);
        }

        public ValueTask<PagedList<WorkflowDefinition>> ListAsync(PagerParameters pagerParameters, CancellationToken cancellationToken = default)
        {
            var pagedList = _workflows.Values.Paginate(pagerParameters);
            return ValueTask.FromResult(pagedList);
        }

        private IEnumerable<WorkflowDefinition> FindManyById(IEnumerable<string> ids)
        {
            var idList = ids as ICollection<string> ?? ids.ToHashSet();
            var keys = _workflows.Keys.Where(x => idList.Contains(x)).ToList();

            foreach (var workflow in keys.Select(key => _workflows[key]))
                yield return workflow;
        }

        private IEnumerable<WorkflowDefinition> CreateWorkflowDefinitions() => _options.Workflows.Values.Select(BuildWorkflowDefinition).ToList();

        private WorkflowDefinition BuildWorkflowDefinition(IWorkflow workflow)
        {
            var builder = new WorkflowDefinitionBuilder();
            builder.WithId(workflow.GetType().Name);
            workflow.Build(builder);
            
            var workflowModel = builder.BuildWorkflow();
            _identityGraphService.AssignIdentities(workflowModel.Workflow);
            
            return workflowModel;
        }
    }
}