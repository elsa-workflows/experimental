using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Services
{
    public class WorkflowInstructionExecutor : IWorkflowInstructionExecutor
    {
        private readonly IEnumerable<IWorkflowInstructionHandler> _workflowExecutionInstructionHandlers;

        public WorkflowInstructionExecutor(IEnumerable<IWorkflowInstructionHandler> workflowExecutionInstructionHandlers)
        {
            _workflowExecutionInstructionHandlers = workflowExecutionInstructionHandlers;
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        {
            var handlers = _workflowExecutionInstructionHandlers.Where(x => x.GetSupportsInstruction(instruction)).ToList();
            var tasks = handlers.Select(x => x.ExecuteInstructionAsync(instruction, cancellationToken).AsTask());
            return await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> ExecuteInstructionsAsync(IEnumerable<IWorkflowInstruction> instructions, CancellationToken cancellationToken = default)
        {
            var tasks = instructions.Select(x => ExecuteInstructionAsync(x, cancellationToken));
            return (await Task.WhenAll(tasks)).SelectMany(x => x);
        }
    }
}