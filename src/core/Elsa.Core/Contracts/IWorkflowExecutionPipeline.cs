using System;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IWorkflowExecutionPipeline
    {
        WorkflowMiddlewareDelegate Setup(Action<IWorkflowExecutionBuilder> setup);
        Task ExecuteAsync(WorkflowExecutionContext context);
    }
}