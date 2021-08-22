using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public IEnumerable<Workflow> GetWorkflowsAsync(CancellationToken cancellationToken = default) => _options.WorkflowFactories.Select(factory => factory());
    }
}