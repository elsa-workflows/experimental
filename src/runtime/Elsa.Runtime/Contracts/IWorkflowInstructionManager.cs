using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionManager
    {
        Task<IEnumerable<IWorkflowInstruction>> GetExecutionInstructionsAsync(IStimulus stimulus, CancellationToken cancellationToken = default);
        Task<IEnumerable<IWorkflowInstructionResult>> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default);
        Task<IEnumerable<IWorkflowInstructionResult>> ExecuteInstructionsAsync(IEnumerable<IWorkflowInstruction> instructions, CancellationToken cancellationToken = default);
    }
}