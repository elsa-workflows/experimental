using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;

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

        public async Task<IEnumerable<IWorkflowInstruction>> GetInstructionsAsync(IStimulus stimulus, CancellationToken cancellationToken = default)
        {
            var providers = _workflowExecutionInstructionProviders.Where(x => x.GetSupportsStimulus(stimulus)).ToList();
            var tasks = providers.Select(x => x.GetInstructionsAsync(stimulus, cancellationToken).AsTask());
            return (await Task.WhenAll(tasks)).SelectMany(x => x);
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> ExecuteInstructionAsync(IWorkflowInstruction instruction, ExecuteInstructionOptions options, CancellationToken cancellationToken = default)
        {
            var handlers = _workflowExecutionInstructionHandlers.Where(x => x.GetSupportsInstruction(instruction)).ToList();
            var tasks = handlers.Select(x => x.ExecuteAsync(instruction, options, cancellationToken).AsTask());
            return await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> ExecuteInstructionsAsync(IEnumerable<IWorkflowInstruction> instructions, ExecuteInstructionOptions options, CancellationToken cancellationToken = default)
        {
            var tasks = instructions.Select(x => ExecuteInstructionAsync(x, options, cancellationToken));
            return (await Task.WhenAll(tasks)).SelectMany(x => x);
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> TriggerWorkflowsAsync(IStimulus stimulus, ExecuteInstructionOptions options, CancellationToken cancellationToken = default)
        {
            var instructions = await GetInstructionsAsync(stimulus, cancellationToken);
            return await ExecuteInstructionsAsync(instructions, options, cancellationToken);
        }
    }
}