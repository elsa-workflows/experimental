using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Elsa.Runtime.Options;
using Elsa.Runtime.Services;
using Microsoft.Extensions.Options;

namespace Elsa.Runtime.Providers
{
    public class ConfigurationWorkflowProvider : IWorkflowProvider
    {
        private readonly WorkflowRuntimeOptions _options;
        public ConfigurationWorkflowProvider(IOptions<WorkflowRuntimeOptions> options) => _options = options.Value;

        public ValueTask<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!_options.Workflows.TryGetValue(id, out var workflow))
                return ValueTask.FromResult<WorkflowDefinition?>(default);

            var builder = new WorkflowBuilder();
            workflow.Build(builder);

            var definition = new WorkflowDefinition
            {
                Id = workflow.Id,
                Version = workflow.Version,
                Root = builder.Root,
                Triggers = builder.Triggers
            };

            return ValueTask.FromResult<WorkflowDefinition?>(definition);
        }

        public ValueTask<IEnumerable<WorkflowDefinition>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            var workflowDefinitions = FindManyById(ids);
            return ValueTask.FromResult(workflowDefinitions);
        }

        private IEnumerable<WorkflowDefinition> FindManyById(IEnumerable<string> ids)
        {
            var idList = ids as ICollection<string> ?? ids.ToHashSet();
            var keys = _options.Workflows.Keys.Where(x => idList.Contains(x)).ToList();

            foreach (var workflow in keys.Select(key => _options.Workflows[key]))
                yield return BuildWorkflowDefinition(workflow);
        }

        private WorkflowDefinition BuildWorkflowDefinition(IWorkflow workflow)
        {
            var builder = new WorkflowBuilder();
            workflow.Build(builder);

            return new WorkflowDefinition
            {
                Id = workflow.Id,
                Version = workflow.Version,
                Root = builder.Root,
                Triggers = builder.Triggers
            };
        }
    }
}