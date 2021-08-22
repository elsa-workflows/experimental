using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Runtime.Providers
{
    public class ConfigurationWorkflowProvider : IWorkflowProvider
    {
        private readonly WorkflowRuntimeOptions _options;
        public ConfigurationWorkflowProvider(IOptions<WorkflowRuntimeOptions> options) => _options = options.Value;

        public ValueTask<IActivity?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var workflow = !_options.WorkflowFactories.TryGetValue(id, out var factory) ? default : factory();
            return new ValueTask<IActivity?>(workflow);
        }
    }
}