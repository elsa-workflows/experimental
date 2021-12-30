using System.Threading;
using System.Threading.Tasks;
using Elsa.Management.Serialization;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
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
        [FromQuery] int pageSize = 50,
        [FromQuery] string? searchTerm = default,
        [FromQuery] string? definitionId = default,
        [FromQuery] string? correlationId = default,
        [FromQuery] int version = -1,
        [FromQuery] WorkflowStatus workflowStatus = default,
        [FromQuery] OrderBy orderBy = OrderBy.Created,
        [FromQuery] OrderDirection orderDirection = OrderDirection.Ascending)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var skip = page * pageSize;
        var take = pageSize;
        var request = new ListWorkflowInstanceSummaries(searchTerm, definitionId, version == -1 ? null : version, correlationId, workflowStatus, orderBy, orderDirection, skip, take);
        var summaries = await requestSender.RequestAsync(request, cancellationToken);

        return Results.Json(summaries, serializerOptions, statusCode: StatusCodes.Status200OK);
    }
}