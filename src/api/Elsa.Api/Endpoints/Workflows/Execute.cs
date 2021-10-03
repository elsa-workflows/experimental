using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.ApiResults;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows
{
    public static class Execute
    {
        public static async Task<IResult> HandleAsync(string definitionId, IWorkflowRegistry workflowRegistry, HttpResponse response, CancellationToken cancellationToken)
        {
            var workflowDefinition = await workflowRegistry.GetByIdAsync(definitionId, cancellationToken);
            return workflowDefinition == null ? Results.NotFound() : new ExecuteWorkflowResult(workflowDefinition);
        }
    }
}