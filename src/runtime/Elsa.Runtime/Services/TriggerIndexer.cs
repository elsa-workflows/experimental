using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Services
{
    public class TriggerIndexer : ITriggerIndexer
    {
        private readonly IEnumerable<ITriggerProvider> _triggerProviders;
        private readonly IHasher _hasher;
        private readonly IWorkflowTriggerStore _workflowTriggerStore;
        private readonly ILogger _logger;

        public TriggerIndexer(IEnumerable<ITriggerProvider> triggerProviders, IHasher hasher, IWorkflowTriggerStore workflowTriggerStore, ILogger<TriggerIndexer> logger)
        {
            _triggerProviders = triggerProviders;
            _hasher = hasher;
            _workflowTriggerStore = workflowTriggerStore;
            _logger = logger;
        }

        public async Task IndexTriggersAsync(Workflow workflow, CancellationToken cancellationToken = default)
        {
            // Collect new triggers.
            var triggers = await GetTriggersAsync(workflow, cancellationToken).ToListAsync(cancellationToken);
            
            // Replace triggers for the specified workflow.
            await _workflowTriggerStore.ReplaceTriggersAsync(workflow.Id, triggers, cancellationToken);
        }

        private async IAsyncEnumerable<WorkflowTrigger> GetTriggersAsync(Workflow workflow, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var triggerSources = workflow.Triggers;

            foreach (var triggerSource in triggerSources)
            {
                var triggers = await GetTriggersAsync(workflow, triggerSource, cancellationToken);

                foreach (var trigger in triggers)
                    yield return trigger;
            }
        }

        private async Task<IEnumerable<WorkflowTrigger>> GetTriggersAsync(Workflow workflow, TriggerSource triggerSource, CancellationToken cancellationToken)
        {
            var activity = triggerSource.Activity;
            var triggerProvider = _triggerProviders.FirstOrDefault(x => x.GetSupportsActivity(activity));

            if (triggerProvider == null)
            {
                _logger.LogWarning("No trigger provider found for trigger source {ActivityType}", activity.ActivityType);
                return ArraySegment<WorkflowTrigger>.Empty;
            }

            var hashInputs = await triggerProvider.GetHashInputsAsync(activity, cancellationToken);

            var triggers = hashInputs.Select(x => new WorkflowTrigger
            {
                Id = Guid.NewGuid().ToString(),
                WorkflowDefinitionId = workflow.Id,
                Name = triggerSource.Activity.ActivityType,
                ActivityId = triggerSource.Activity.ActivityId,
                Hash = _hasher.Hash(x)
            });

            return triggers;
        }
    }
}