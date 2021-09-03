using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows
{
    public static class Execute
    {
        public static async Task<IResult> HandleAsync(string id, IWorkflowStore workflowStore, IActivityInvoker activityInvoker, HttpResponse response, CancellationToken cancellationToken)
        {
            var workflow = await workflowStore.FindByIdAsync(id, cancellationToken);

            if (workflow == null)
                return Results.NotFound();

            await activityInvoker.InvokeAsync(workflow, cancellationToken: cancellationToken);
            return response.HasStarted ? new EmptyResult() : Results.Ok($"Executed {id}!");
        }

        public class EmptyResult : IResult
        {
            public Task ExecuteAsync(HttpContext httpContext) => Task.CompletedTask;
        }
    }
}