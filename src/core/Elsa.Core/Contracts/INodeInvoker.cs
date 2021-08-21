using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeInvoker
    {
        Task<WorkflowExecutionContext> ResumeAsync(Bookmark bookmark, INode root, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionContext> InvokeAsync(INode node, INode? root = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default);
        Task<WorkflowExecutionContext> InvokeAsync(ScheduledNode scheduledNode, INode? root = default, ExecuteNodeDelegate? executeNodeDelegate = default, CancellationToken cancellationToken = default);
    }
}