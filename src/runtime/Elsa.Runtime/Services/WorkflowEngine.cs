using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.State;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Runtime.Services
{
    /// <summary>
    /// A workflow engine represents a logical unit of services to execute workflows.
    /// It's basically a thin wrapper around a service provider built from a given service collection using <see cref="WorkflowEngineBuilder"/>.
    /// Useful for scenarios where you have multiple tenants in a system or if you simply want to use different workflow engine configurations within the same application.
    /// </summary>
    public class WorkflowEngine : IWorkflowEngine
    {
        public WorkflowEngine(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
        
        public async Task<WorkflowExecutionResult> ExecuteWorkflowAsync(Workflow workflow, CancellationToken cancellationToken = default)
        {
            var workflowInvoker = ServiceProvider.GetRequiredService<IWorkflowInvoker>();
            return await workflowInvoker.InvokeAsync(workflow, cancellationToken);
        }

        public async Task<IEnumerable<WorkflowInstructionResult?>> TriggerWorkflowsAsync(IStimulus stimulus, CancellationToken cancellationToken = default)
        {
            var stimulusInterpreter = ServiceProvider.GetRequiredService<IStimulusInterpreter>();
            var instructions = await stimulusInterpreter.GetExecutionInstructionsAsync(stimulus, cancellationToken);
            var instructionExecutor = ServiceProvider.GetRequiredService<IWorkflowInstructionExecutor>();
            return await instructionExecutor.ExecuteInstructionsAsync(instructions, cancellationToken);
        }

        public async Task<WorkflowExecutionResult> ResumeAsync(Workflow workflow, Bookmark bookmark, WorkflowState workflowState, CancellationToken cancellationToken = default)
        {
            var workflowInvoker = ServiceProvider.GetRequiredService<IWorkflowInvoker>();
            return await workflowInvoker.ResumeAsync(workflow, bookmark, workflowState, cancellationToken);
        }
    }
}