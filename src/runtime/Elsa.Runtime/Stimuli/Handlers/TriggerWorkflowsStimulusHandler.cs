using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;

namespace Elsa.Runtime.Stimuli.Handlers;

public class TriggerWorkflowsStimulusHandler : StimulusHandler<StandardStimulus>
{
    private readonly IWorkflowTriggerStore _workflowTriggerStore;
    public TriggerWorkflowsStimulusHandler(IWorkflowTriggerStore workflowTriggerStore) => _workflowTriggerStore = workflowTriggerStore;

    protected override async ValueTask<IEnumerable<IWorkflowInstruction>> GetInstructionsAsync(StandardStimulus stimulus, CancellationToken cancellationToken = default)
    {
        var workflowTriggers = (await _workflowTriggerStore.FindManyAsync(stimulus.ActivityTypeName, stimulus.Hash, cancellationToken)).ToList();
        return workflowTriggers.Select(x => new TriggerWorkflowInstruction(x));
    }
}