using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Extensions;
using Elsa.Runtime.Models;
using Elsa.Runtime.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Runtime.WorkflowProviders
{
    public class ConfigurationWorkflowProvider : IWorkflowProvider
    {
        private readonly IIdentityGraphService _identityGraphService;
        private readonly WorkflowRuntimeOptions _options;
        private readonly ICollection<Workflow> _workflows;

        public ConfigurationWorkflowProvider(IOptions<WorkflowRuntimeOptions> options, IIdentityGraphService identityGraphService)
        {
            _identityGraphService = identityGraphService;
            _options = options.Value;
            _workflows = CreateWorkflowDefinitions().ToList();
        }

        public ValueTask<Workflow?> FindByIdAsync(string id, VersionOptions versionOptions, CancellationToken cancellationToken = default)
        {
            var result = _workflows.FirstOrDefault(x => x.Metadata.Identity.Id == id && x.WithVersion(versionOptions));
            return ValueTask.FromResult(result);
        }

        public IAsyncEnumerable<Workflow> StreamAllAsync(CancellationToken cancellationToken = default) => _workflows.ToAsyncEnumerable();

        private IEnumerable<Workflow> CreateWorkflowDefinitions() => _options.Workflows.Values.Select(BuildWorkflowDefinition).ToList();

        private Workflow BuildWorkflowDefinition(IWorkflow workflowDeclaration)
        {
            var builder = new WorkflowDefinitionBuilder();
            builder.WithId(workflowDeclaration.GetType().Name);
            workflowDeclaration.Build(builder);

            var workflow = builder.BuildWorkflow();
            _identityGraphService.AssignIdentities(workflow);

            return workflow;
        }
    }
}