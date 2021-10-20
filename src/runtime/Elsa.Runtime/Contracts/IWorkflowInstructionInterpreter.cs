using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowInstructionInterpreter
    {
        bool GetSupportsInstruction(IWorkflowInstruction instruction);
        ValueTask<WorkflowInstructionResult?> ExecuteAsync(IWorkflowInstruction instruction, ExecuteInstructionOptions options, CancellationToken cancellationToken = default);
    }
}