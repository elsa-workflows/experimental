using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.InstructionResults;

namespace Elsa.Runtime.Abstractions
{
    public abstract class WorkflowInstructionHandler<TInstruction> : IWorkflowInstructionHandler where TInstruction: IWorkflowInstruction
    {
        bool IWorkflowInstructionHandler.GetSupportsInstruction(IWorkflowInstruction instruction) => instruction is TInstruction;

        ValueTask<IWorkflowInstructionResult> IWorkflowInstructionHandler.ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken) => ExecuteInstructionAsync((TInstruction)instruction, cancellationToken);

        protected virtual ValueTask<IWorkflowInstructionResult> ExecuteInstructionAsync(TInstruction instruction, CancellationToken cancellationToken = default)
        {
            var result = ExecuteInstruction(instruction);
            return ValueTask.FromResult(result);
        }

        protected virtual IWorkflowInstructionResult ExecuteInstruction(TInstruction instruction) => NullInstructionResult.Instance;
    }
}