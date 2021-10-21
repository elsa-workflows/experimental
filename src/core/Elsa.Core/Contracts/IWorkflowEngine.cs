using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IWorkflowEngine
    {
        Task<ExecuteWorkflowResult> ExecuteAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default);
        Task<ExecuteWorkflowResult> ExecuteAsync(WorkflowExecutionContext workflowExecutionContext);
        Task<ExecuteWorkflowResult> ExecuteAsync(WorkflowDefinition workflowDefinition, WorkflowState workflowState, Bookmark? bookmark = default, CancellationToken cancellationToken = default);

        WorkflowExecutionContext CreateWorkflowExecutionContext(
            IServiceProvider serviceProvider,
            WorkflowDefinition workflow,
            WorkflowState? workflowState = default,
            Bookmark? bookmark = default,
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default);
    }
}