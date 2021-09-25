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
        private readonly IActivityWalker _activityWalker;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IHasher _hasher;
        private readonly IWorkflowTriggerStore _workflowTriggerStore;
        private readonly ILogger _logger;

        public TriggerIndexer(
            IActivityWalker activityWalker,
            IExpressionEvaluator expressionEvaluator,
            IHasher hasher,
            IWorkflowTriggerStore workflowTriggerStore,
            ILogger<TriggerIndexer> logger)
        {
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
            var context = new WorkflowIndexingContext(workflow);
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
            var inputs = trigger.GetInputs();
            var assignedInputs = inputs.Where(x => x.LocationReference != null!).ToList();
            var register = context.GetOrCreateRegister(trigger);
            var expressionExecutionContext = new ExpressionExecutionContext(register, default);

            // Evaluate trigger inputs.
            foreach (var input in assignedInputs)
            {
                var locationReference = input.LocationReference;
                var value = await _expressionEvaluator.EvaluateAsync(input.Expression, expressionExecutionContext);
                locationReference.Set(expressionExecutionContext, value);
            }

            var triggerIndexingContext = new TriggerIndexingContext(context, expressionExecutionContext, trigger);
            var hashInputs = await trigger.GetHashInputsAsync(triggerIndexingContext, cancellationToken);

            var triggers = hashInputs.Select(x => new WorkflowTrigger
            {
                Id = Guid.NewGuid().ToString(),
                WorkflowDefinitionId = context.Workflow.Id,
                Name = trigger.GetType().Name,
                Hash = _hasher.Hash(x)
            });

            return triggers;
        }
    }
}