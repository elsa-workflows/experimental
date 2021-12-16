using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.ApiResults;
using Elsa.Persistence.Models;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public static async Task<IResult> ExecuteAsync(string id, IWorkflowRegistry workflowRegistry, HttpResponse response, CancellationToken cancellationToken)
    {
        var workflow = await workflowRegistry.FindByIdAsync(id, VersionOptions.Published, cancellationToken);
        return workflow == null ? Results.NotFound() : new ExecuteWorkflowResult(workflow);
    }
}