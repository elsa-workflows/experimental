using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Abstractions
{
    public abstract class WorkflowInstructionInterpreter<TInstruction> : IWorkflowInstructionInterpreter where TInstruction: IWorkflowInstruction
    {
        bool IWorkflowInstructionInterpreter.GetSupportsInstruction(IWorkflowInstruction instruction) => instruction is TInstruction;

        ValueTask<WorkflowInstructionResult?> IWorkflowInstructionInterpreter.ExecuteAsync(IWorkflowInstruction instruction, ExecuteInstructionOptions options, CancellationToken cancellationToken) => ExecuteInstructionAsync((TInstruction)instruction, options, cancellationToken);

        protected virtual ValueTask<WorkflowInstructionResult?> ExecuteInstructionAsync(TInstruction instruction, ExecuteInstructionOptions options, CancellationToken cancellationToken = default)
        {
            var result = ExecuteInstruction(instruction, options);
            return ValueTask.FromResult(result);
        }

        protected virtual WorkflowInstructionResult? ExecuteInstruction(TInstruction instruction, ExecuteInstructionOptions options) => null;
    }
}