using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Management.Serialization;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Models;
using Elsa.Persistence.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public static async Task<IResult> Get(
        IRequestSender requestSender,
        WorkflowSerializerOptionsProvider serializerOptionsProvider,
        CancellationToken cancellationToken,
        [FromRoute] string definitionId,
        [FromQuery] string? versionOptions = default)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var parsedVersionOptions = versionOptions != null ? VersionOptions.FromString(versionOptions) : VersionOptions.Latest;
        var workflowDefinition = await requestSender.RequestAsync(new FindWorkflowDefinition(definitionId, parsedVersionOptions), cancellationToken);
        return workflowDefinition == null ? Results.NotFound() : Results.Json(workflowDefinition, serializerOptions, statusCode: StatusCodes.Status200OK);
    }
}