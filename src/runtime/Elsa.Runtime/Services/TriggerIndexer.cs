using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Mediator.Contracts;
using Elsa.Models;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Services;

public class TriggerIndexer : ITriggerIndexer
{
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ICommandSender _mediator;
    private readonly IHasher _hasher;

    public TriggerIndexer(
        IExpressionEvaluator expressionEvaluator,
        ICommandSender mediator,
        IHasher hasher)
    {
        _expressionEvaluator = expressionEvaluator;
        _mediator = mediator;
        _hasher = hasher;
    }

    public async Task IndexTriggersAsync(Workflow workflow, CancellationToken cancellationToken = default)
    {
        // Collect new triggers.
        var triggers = await GetTriggersAsync(workflow, cancellationToken).ToListAsync(cancellationToken);

        // Replace triggers for the specified workflow.
        var definitionId = workflow.Identity.DefinitionId;
        await _mediator.ExecuteAsync(new ReplaceWorkflowTriggers(definitionId, triggers), cancellationToken);
    }

    private async IAsyncEnumerable<WorkflowTrigger> GetTriggersAsync(Workflow workflow, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var context = new WorkflowIndexingContext(workflow);
        var triggerSources = workflow.Triggers ?? Enumerable.Empty<ITrigger>();

        foreach (var triggerSource in triggerSources)
        {
            var triggers = await GetTriggersAsync(workflow, context, triggerSource, cancellationToken);

            foreach (var trigger in triggers)
                yield return trigger;
        }
    }

    private async Task<IEnumerable<WorkflowTrigger>> GetTriggersAsync(Workflow workflow, WorkflowIndexingContext context, ITrigger trigger, CancellationToken cancellationToken)
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
            WorkflowDefinitionId = workflow.Identity.DefinitionId,
            Name = trigger.GetType().Name,
            Hash = _hasher.Hash(x)
        });

        return triggers;
    }
}