using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.ApiResults
{
    public class ExecuteWorkflowResult : IResult
    {
        public ExecuteWorkflowResult(WorkflowDefinition workflowDefinition) => WorkflowDefinition = workflowDefinition;
        public WorkflowDefinition WorkflowDefinition { get; }
        
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var response = httpContext.Response;
            var workflowInvoker = httpContext.RequestServices.GetRequiredService<IWorkflowExecutor>();
            var result = await workflowInvoker.ExecuteAsync(WorkflowDefinition);

            if (!response.HasStarted) 
                await response.WriteAsJsonAsync(result, httpContext.RequestAborted);
        }
    }
}