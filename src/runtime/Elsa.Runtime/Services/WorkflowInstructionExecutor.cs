using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Services
{
    public class WorkflowInstructionExecutor : IWorkflowInstructionExecutor
    {
        private readonly IEnumerable<IWorkflowInstructionInterpreter> _workflowExecutionInstructionHandlers;

        public WorkflowInstructionExecutor(IEnumerable<IWorkflowInstructionInterpreter> workflowExecutionInstructionHandlers)
        {
            _workflowExecutionInstructionHandlers = workflowExecutionInstructionHandlers;
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> ExecuteAsync(IWorkflowInstruction instruction, ExecuteInstructionOptions options, CancellationToken cancellationToken = default)
        {
            var handlers = _workflowExecutionInstructionHandlers.Where(x => x.GetSupportsInstruction(instruction)).ToList();
            var tasks = handlers.Select(x => x.ExecuteAsync(instruction, options, cancellationToken).AsTask());
            return await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> ExecuteAsync(IEnumerable<IWorkflowInstruction> instructions, ExecuteInstructionOptions options, CancellationToken cancellationToken = default)
        {
            var tasks = instructions.Select(x => ExecuteAsync(x, options, cancellationToken));
            return (await Task.WhenAll(tasks)).SelectMany(x => x);
        }
    }
}