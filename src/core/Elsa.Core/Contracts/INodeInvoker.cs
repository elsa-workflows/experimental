using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Models.State;

namespace Elsa.Contracts
{
    public interface INodeInvoker
    {
        Task<WorkflowExecutionContext> ResumeAsync(string bookmarkName, INode root, WorkflowState workflowState, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionContext> InvokeAsync(INode node, INode? root = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default);
    }
}