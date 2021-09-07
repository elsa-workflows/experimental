using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.ApiResults;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows
{
    public static class Execute
    {
        public static async Task<IResult> HandleAsync(string id, IWorkflowRegistry workflowRegistry, HttpResponse response, CancellationToken cancellationToken)
        {
            var workflow = await workflowRegistry.GetByIdAsync(id, cancellationToken);
            return workflow == null ? Results.NotFound() : new ExecuteWorkflowResult(workflow);
        }
    }
}