using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Abstractions
{
    public abstract class WorkflowInstructionInterpreter<TInstruction> : IWorkflowInstructionInterpreter where TInstruction: IWorkflowInstruction
    {
        bool IWorkflowInstructionInterpreter.GetSupportsInstruction(IWorkflowInstruction instruction) => instruction is TInstruction;

        ValueTask<WorkflowInstructionResult?> IWorkflowInstructionInterpreter.ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken) => ExecuteInstructionAsync((TInstruction)instruction, cancellationToken);

        protected virtual ValueTask<WorkflowInstructionResult?> ExecuteInstructionAsync(TInstruction instruction, CancellationToken cancellationToken = default)
        {
            var result = ExecuteInstruction(instruction);
            return ValueTask.FromResult(result);
        }

        protected virtual WorkflowInstructionResult? ExecuteInstruction(TInstruction instruction) => null;
    }
}