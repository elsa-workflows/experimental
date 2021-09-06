using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
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
            var workflowManager = httpContext.RequestServices.GetRequiredService<IWorkflowManager>();
            var result = await workflowManager.ExecuteWorkflowAsync(Workflow);

            if (!response.HasStarted) 
                await response.WriteAsJsonAsync(result, httpContext.RequestAborted);
        }
    }
}