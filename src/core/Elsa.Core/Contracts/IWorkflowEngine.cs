using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.State;

namespace Elsa.Contracts
{
    public interface IWorkflowEngine
    {
        Task<ExecuteWorkflowResult> ExecuteAsync(Workflow workflow, CancellationToken cancellationToken = default);
        Task<ExecuteWorkflowResult> ExecuteAsync(WorkflowExecutionContext workflowExecutionContext);
        Task<ExecuteWorkflowResult> ExecuteAsync(Workflow workflow, WorkflowState workflowState, Bookmark? bookmark = default, CancellationToken cancellationToken = default);

        WorkflowExecutionContext CreateWorkflowExecutionContext(
            IServiceProvider serviceProvider,
            Workflow workflow,
            WorkflowState? workflowState = default,
            Bookmark? bookmark = default,
            ExecuteActivityDelegate? executeActivityDelegate = default,
            CancellationToken cancellationToken = default);
    }
}