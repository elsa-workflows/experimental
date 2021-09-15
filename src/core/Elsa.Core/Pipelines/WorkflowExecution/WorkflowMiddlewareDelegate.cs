using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Pipelines.WorkflowExecution
{
    public delegate ValueTask WorkflowMiddlewareDelegate(WorkflowExecutionContext context);
}