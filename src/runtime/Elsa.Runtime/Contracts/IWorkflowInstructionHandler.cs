using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Instructions;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionHandler
    {
        bool GetSupportsInstruction(IWorkflowInstruction instruction);
        ValueTask<IWorkflowInstructionResult> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default);
    }
}