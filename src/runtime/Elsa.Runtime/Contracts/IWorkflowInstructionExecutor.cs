using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionExecutor
    {
        Task<IEnumerable<WorkflowInstructionResult?>> ExecuteAsync(IWorkflowInstruction instruction, ExecuteInstructionOptions options, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowInstructionResult?>> ExecuteAsync(IEnumerable<IWorkflowInstruction> instructions, ExecuteInstructionOptions options, CancellationToken cancellationToken = default);
    }
}