using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionInterpreter
    {
        bool GetSupportsInstruction(IWorkflowInstruction instruction);
        ValueTask<WorkflowInstructionResult?> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default);
    }
}