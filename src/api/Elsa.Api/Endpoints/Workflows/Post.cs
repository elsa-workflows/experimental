using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Converters;
using Elsa.Api.Core.Contracts;
using Elsa.Models;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public static async Task<IResult> PostAsync(HttpContext httpContext, IWellKnownTypeRegistry wellKnownTypeRegistry, CancellationToken cancellationToken)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new TypeJsonConverter(wellKnownTypeRegistry),
                new ActivityJsonConverter(),
            }
        };

        var workflow = await httpContext.Request.ReadFromJsonAsync<Workflow>(serializerOptions, cancellationToken);
        return Results.Json(workflow, serializerOptions);
    }
}