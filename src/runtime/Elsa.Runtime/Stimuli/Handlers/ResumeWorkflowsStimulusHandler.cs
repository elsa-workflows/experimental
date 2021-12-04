using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Abstractions;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Instructions;

namespace Elsa.Runtime.Stimuli.Handlers;

public class ResumeWorkflowsStimulusHandler : StimulusHandler<StandardStimulus>
{
    private readonly IWorkflowBookmarkStore _workflowBookmarkStore;
    public ResumeWorkflowsStimulusHandler(IWorkflowBookmarkStore workflowBookmarkStore) => _workflowBookmarkStore = workflowBookmarkStore;

    protected override async ValueTask<IEnumerable<IWorkflowInstruction>> GetInstructionsAsync(StandardStimulus stimulus, CancellationToken cancellationToken = default)
    {
        var workflowBookmarks = (await _workflowBookmarkStore.FindManyAsync(stimulus.ActivityTypeName, stimulus.Hash, cancellationToken)).ToList();
        return workflowBookmarks.Select(x => new ResumeWorkflowInstruction(x));
    }
}