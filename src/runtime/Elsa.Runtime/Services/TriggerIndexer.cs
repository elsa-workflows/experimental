using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.Services
{
    public class TriggerIndexer : ITriggerIndexer
    {
        private readonly IActivityDriverActivator _activityDriverActivator;
        private readonly IHasher _hasher;
        private readonly ILogger _logger;

        public TriggerIndexer(IActivityDriverActivator activityDriverActivator, IHasher hasher, ILogger<TriggerIndexer> logger)
        {
            _activityDriverActivator = activityDriverActivator;
            _hasher = hasher;
            _logger = logger;
        }

        public async Task<IEnumerable<WorkflowTrigger>> IndexTriggersAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default) =>
            await IndexTriggersInternalAsync(workflowDefinition, cancellationToken).ToListAsync(cancellationToken);

        private async IAsyncEnumerable<WorkflowTrigger> IndexTriggersInternalAsync(WorkflowDefinition workflowDefinition, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var triggerSources = workflowDefinition.Triggers;

            foreach (var triggerSource in triggerSources)
            {
                var triggers = await GetTriggers(workflowDefinition, triggerSource, cancellationToken);

                foreach (var trigger in triggers)
                    yield return trigger;
            }
        }

        private async Task<IEnumerable<WorkflowTrigger>> GetTriggers(WorkflowDefinition workflowDefinition, TriggerSource triggerSource, CancellationToken cancellationToken)
        {
            var activity = triggerSource.Activity;
            var driver = _activityDriverActivator.GetDriver(activity);

            if (driver == null)
            {
                _logger.LogWarning("No driver found for trigger source {ActivityType}", activity.ActivityType);
                return ArraySegment<WorkflowTrigger>.Empty;
            }

            if (driver is not ITriggerProvider triggerProvider)
            {
                _logger.LogWarning("Driver for trigger source {ActivityType} does not implement ITriggerProvider", activity.ActivityType);
                return ArraySegment<WorkflowTrigger>.Empty;
            }

            var hashInputs = await triggerProvider.GetHashInputsAsync(activity, cancellationToken);

            var triggers = hashInputs.Select(x => new WorkflowTrigger
            {
                WorkflowDefinitionId = workflowDefinition.DefinitionId,
                ActivityTypeName = triggerSource.Activity.ActivityType,
                ActivityId = triggerSource.Activity.ActivityId,
                Hash = _hasher.Hash(x)
            });

            return triggers;
        }
    }
}