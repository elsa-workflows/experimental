using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.ApiResults
{
    public class ExecuteWorkflowResult : IResult
    {
        public ExecuteWorkflowResult(Workflow workflow) => Workflow = workflow;
        public Workflow Workflow { get; }
        
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var response = httpContext.Response;
            var workflowInvoker = httpContext.RequestServices.GetRequiredService<IWorkflowInvoker>();
            var result = await workflowInvoker.InvokeAsync(Workflow);

            if (!response.HasStarted) 
                await response.WriteAsJsonAsync(result, httpContext.RequestAborted);
        }
    }
}