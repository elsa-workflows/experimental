using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowEngine
    {
        IServiceProvider ServiceProvider { get; }
        Task<WorkflowExecutionResult> ExecuteWorkflowAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionResult> ResumeAsync(WorkflowDefinition workflowDefinition, Bookmark bookmark, WorkflowState workflowState, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowInstructionResult?>> TriggerWorkflowsAsync(IStimulus stimulus, CancellationToken cancellationToken);
        
    }
}