using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Api.Endpoints.Workflows
{
    public static class Execute
    {
        public static async Task<IActionResult> HandleAsync(string id, IWorkflowStore workflowStore, IActivityInvoker activityInvoker, CancellationToken cancellationToken)
        {
            var workflow = await workflowStore.FindByIdAsync(id, cancellationToken);

            if (workflow == null)
                return new NotFoundResult();

            await activityInvoker.InvokeAsync(workflow, cancellationToken: cancellationToken);
            
            return new OkObjectResult($"Executed {id}!");
        } 
    }
}