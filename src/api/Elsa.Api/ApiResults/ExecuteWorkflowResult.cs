using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
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
            var workflowInvoker = httpContext.RequestServices.GetRequiredService<IWorkflowInvoker>();
            var executeRequest = new ExecuteWorkflowDefinitionRequest(WorkflowDefinition.DefinitionId, WorkflowDefinition.Version); 
            var result = await workflowInvoker.ExecuteAsync(executeRequest, CancellationToken.None);

            if (!response.HasStarted) 
                await response.WriteAsJsonAsync(result, httpContext.RequestAborted);
        }
    }
}