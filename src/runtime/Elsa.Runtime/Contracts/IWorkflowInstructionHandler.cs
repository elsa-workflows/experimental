using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionHandler
    {
        bool GetSupportsInstruction(IWorkflowInstruction instruction);
        ValueTask<IWorkflowInstructionResult> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default);
    }
}