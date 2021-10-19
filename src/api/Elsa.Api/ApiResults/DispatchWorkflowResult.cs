using System.Net;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.ApiResults
{
    public class DispatchWorkflowResult : IResult
    {
        public DispatchWorkflowResult(WorkflowDefinition workflowDefinition) => WorkflowDefinition = workflowDefinition;
        public WorkflowDefinition WorkflowDefinition { get; }
        
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var response = httpContext.Response;
            var workflowDispatcher = httpContext.RequestServices.GetRequiredService<IWorkflowDefinitionDispatcher>();
            await workflowDispatcher.DispatchAsync(new DispatchWorkflowDefinitionRequest(WorkflowDefinition.DefinitionId, WorkflowDefinition.Version));

            response.StatusCode = (int)HttpStatusCode.OK;
            //if (!response.HasStarted) 
            //await response.WriteAsJsonAsync(result, httpContext.RequestAborted);
        }
    }
}