using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionScheduler
    {
        Task<IEnumerable<WorkflowInstructionResult?>> ScheduleInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowInstructionResult?>> ScheduleInstructionsAsync(IEnumerable<IWorkflowInstruction> instructions, CancellationToken cancellationToken = default);
    }
}