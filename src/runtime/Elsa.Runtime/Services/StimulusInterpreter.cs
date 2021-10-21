using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Services
{
    public class StimulusInterpreter : IStimulusInterpreter
    {
        private readonly IEnumerable<IStimulusHandler> _workflowExecutionInstructionProviders;
        private readonly IEnumerable<IWorkflowInstructionInterpreter> _workflowExecutionInstructionHandlers;

        public StimulusInterpreter(
            IEnumerable<IStimulusHandler> workflowExecutionInstructionProviders,
            IEnumerable<IWorkflowInstructionInterpreter> workflowExecutionInstructionHandlers)
        {
            _workflowExecutionInstructionProviders = workflowExecutionInstructionProviders;
            _workflowExecutionInstructionHandlers = workflowExecutionInstructionHandlers;
        }

        public async Task<IEnumerable<IWorkflowInstruction>> GetExecutionInstructionsAsync(IStimulus stimulus, CancellationToken cancellationToken = default)
        {
            var providers = _workflowExecutionInstructionProviders.Where(x => x.GetSupportsStimulus(stimulus)).ToList();
            var tasks = providers.Select(x => x.GetInstructionsAsync(stimulus, cancellationToken).AsTask());
            return (await Task.WhenAll(tasks)).SelectMany(x => x);
        }

        // public async Task<IEnumerable<ExecuteWorkflowInstructionResult?>> ExecuteInstructionAsync(IWorkflowInstruction instruction, CancellationToken cancellationToken = default)
        // {
        //     var handlers = _workflowExecutionInstructionHandlers.Where(x => x.GetSupportsInstruction(instruction)).ToList();
        //     var tasks = handlers.Select(x => x.ExecuteInstructionAsync(instruction, cancellationToken).AsTask());
        //     return await Task.WhenAll(tasks);
        // }
        //
        // public async Task<IEnumerable<ExecuteWorkflowInstructionResult?>> ExecuteInstructionsAsync(IEnumerable<IWorkflowInstruction> instructions, CancellationToken cancellationToken = default)
        // {
        //     var tasks = instructions.Select(x => ExecuteInstructionAsync(x, cancellationToken));
        //     return (await Task.WhenAll(tasks)).SelectMany(x => x);
        // }
        //
        // public async Task<IEnumerable<ExecuteWorkflowInstructionResult?>> TriggerWorkflowsAsync(IStimulus stimulus, CancellationToken cancellationToken = default)
        // {
        //     var instructions = await GetExecutionInstructionsAsync(stimulus, cancellationToken);
        //     return await ExecuteInstructionsAsync(instructions, cancellationToken);
        // }
    }
}