using System.Threading;
using System.Threading.Tasks;
using Elsa.Management.Serialization;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Api.Endpoints.WorkflowInstances;

public static partial class WorkflowInstances
{
    public static async Task<IResult> ListAsync(
        IRequestSender requestSender,
        WorkflowSerializerOptionsProvider serializerOptionsProvider,
        CancellationToken cancellationToken,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 50)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var skip = page * pageSize;
        var take = pageSize;
        var summaries = await requestSender.RequestAsync(new ListWorkflowInstanceSummaries(skip, take), cancellationToken);

        return Results.Json(summaries, serializerOptions, statusCode: StatusCodes.Status200OK);
    }
}