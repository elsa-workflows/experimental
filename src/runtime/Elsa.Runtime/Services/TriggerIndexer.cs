using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Extensions;
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
        private readonly IActivityWalker _activityWalker;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IHasher _hasher;
        private readonly IWorkflowTriggerStore _workflowTriggerStore;
        private readonly ILogger _logger;

        public TriggerIndexer(
            IEnumerable<ITriggerProvider> triggerProviders, 
            IActivityWalker activityWalker,
            IExpressionEvaluator expressionEvaluator,
            IHasher hasher, 
            IWorkflowTriggerStore workflowTriggerStore, 
            ILogger<TriggerIndexer> logger)
        {
            _triggerProviders = triggerProviders;
            _activityWalker = activityWalker;
            _expressionEvaluator = expressionEvaluator;
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
            var graph = _activityWalker.Walk(workflow.Root);
            var context = new WorkflowIndexingContext(workflow, graph, cancellationToken);
            var triggerSources = workflow.Triggers ?? Enumerable.Empty<ITrigger>();

            foreach (var triggerSource in triggerSources)
            {
                var triggers = await GetTriggersAsync(context, triggerSource, cancellationToken);

                foreach (var trigger in triggers)
                    yield return trigger;
            }
        }

        private async Task<IEnumerable<WorkflowTrigger>> GetTriggersAsync(WorkflowIndexingContext context, ITrigger trigger, CancellationToken cancellationToken)
        {
            var activity = trigger;
            var triggerProvider = _triggerProviders.FirstOrDefault(x => x.GetSupportsActivity(activity));

            if (triggerProvider == null)
            {
                _logger.LogWarning("No trigger provider found for trigger source {ActivityType}", activity.ActivityType);
                return ArraySegment<WorkflowTrigger>.Empty;
            }
            
            // Evaluate activity inputs.
            var inputs = activity.GetInputs();
            var assignedInputs = inputs.Where(x => x.LocationReference != null!).ToList();
            var register = context.GetOrCreateRegister(activity);
            var expressionExecutionContext = new ExpressionExecutionContext(register);
            
            foreach (var input in assignedInputs)
            {
                var locationReference = input.LocationReference;
                
                var value = await _expressionEvaluator.EvaluateAsync(input.Expression, expressionExecutionContext);
                locationReference.Set(expressionExecutionContext, value);
            }

            var triggerIndexingContext = new TriggerIndexingContext(context, expressionExecutionContext, activity);
            var hashInputs = await triggerProvider.GetHashInputsAsync(triggerIndexingContext, cancellationToken);

            var triggers = hashInputs.Select(x => new WorkflowTrigger
            {
                Id = Guid.NewGuid().ToString(),
                WorkflowDefinitionId = context.Workflow.Id,
                Name = trigger.ActivityType,
                ActivityId = trigger.ActivityId,
                Hash = _hasher.Hash(x)
            });

            return triggers;
        }
    }
}