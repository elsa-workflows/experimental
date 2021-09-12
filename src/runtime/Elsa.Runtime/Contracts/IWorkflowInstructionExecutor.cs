using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionExecutor
    {
        Task<IEnumerable<WorkflowInstructionResult?>> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowInstructionResult?>> ExecuteInstructionsAsync(IEnumerable<IWorkflowInstruction> instructions, CancellationToken cancellationToken = default);
    }
}