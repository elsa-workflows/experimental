using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Serialization;
using Elsa.Models;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public static async Task<IResult> PostAsync(HttpContext httpContext, WorkflowSerializerOptionsProvider serializerOptionsProvider, CancellationToken cancellationToken)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var workflow = await httpContext.Request.ReadFromJsonAsync<Workflow>(serializerOptions, cancellationToken);
        return Results.Json(workflow, serializerOptions);
    }
}