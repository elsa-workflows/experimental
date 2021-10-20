using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface IStimulusInterpreter
    {
        Task<IEnumerable<IWorkflowInstruction>> GetInstructionsAsync(IStimulus stimulus, CancellationToken cancellationToken = default);
    }
}