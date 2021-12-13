using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Serialization;
using Elsa.Mediator.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Commands;
using Elsa.Persistence.Abstractions.Entities;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public static async Task<IResult> PostAsync(HttpContext httpContext, WorkflowSerializerOptionsProvider serializerOptionsProvider, IMediator mediator, CancellationToken cancellationToken)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var workflow = (await httpContext.Request.ReadFromJsonAsync<Workflow>(serializerOptions, cancellationToken))!;

        var workflowDefinition = new WorkflowDefinition
        {
            Id = Guid.NewGuid().ToString("N"),
            DefinitionId = workflow.Metadata.Identity.Id,
            Version = workflow.Metadata.Identity.Version,
            CreatedAt = DateTime.UtcNow,
            Root = workflow.Root,
            Triggers = workflow.Triggers
        };

        await mediator.SendCommandAsync(new SaveWorkflowDefinition(workflowDefinition), cancellationToken);
        return Results.Json(workflow, serializerOptions);
    }
}