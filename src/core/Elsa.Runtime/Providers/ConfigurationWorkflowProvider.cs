using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Elsa.Runtime.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Runtime.Providers
{
    public class ConfigurationWorkflowProvider : IWorkflowProvider
    {
        private readonly WorkflowRuntimeOptions _options;
        public ConfigurationWorkflowProvider(IOptions<WorkflowRuntimeOptions> options) => _options = options.Value;

        public ValueTask<WorkflowDefinition?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var workflow = _options.WorkflowFactories.TryGetValue(id, out var factory) ? factory() : default;
            return ValueTask.FromResult(workflow);
        }

        public ValueTask<IEnumerable<WorkflowDefinition>> FindManyByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            var workflowDefinitions = FindManyById(ids);
            return ValueTask.FromResult(workflowDefinitions);
        }
        
        private IEnumerable<WorkflowDefinition> FindManyById(IEnumerable<string> ids)
        {
            var idList = ids as ICollection<string> ?? ids.ToHashSet();
            var keys = _options.WorkflowFactories.Keys.Where(x => idList.Contains(x)).ToList();

            foreach (var factory in keys.Select(key => _options.WorkflowFactories[key]))
                yield return factory();
        }
    }
}