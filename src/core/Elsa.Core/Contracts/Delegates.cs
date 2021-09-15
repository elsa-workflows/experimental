using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public delegate ValueTask ActivityMiddlewareDelegate(ActivityExecutionContext context);
    public delegate ValueTask WorkflowMiddlewareDelegate(WorkflowExecutionContext context);
    public delegate ValueTask ExecuteActivityDelegate(ActivityExecutionContext context);
    public delegate ValueTask ActivityCompletionCallback(ActivityExecutionContext completedActivityExecutionContext, ActivityExecutionContext ownerActivityExecutionContext);
}