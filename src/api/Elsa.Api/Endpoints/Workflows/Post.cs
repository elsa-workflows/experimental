using System.Threading;
using System.Threading.Tasks;
using Elsa.Management.Contracts;
using Elsa.Management.Serialization;
using Elsa.Models;
using Elsa.Persistence.Mappers;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public record SaveWorkflowRequest(Workflow Workflow, bool Publish);

    public static async Task<IResult> PostAsync(
        HttpContext httpContext,
        WorkflowSerializerOptionsProvider serializerOptionsProvider,
        IWorkflowPublisher workflowPublisher,
        WorkflowDefinitionMapper mapper,
        CancellationToken cancellationToken)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var model = (await httpContext.Request.ReadFromJsonAsync<SaveWorkflowRequest>(serializerOptions, cancellationToken))!;
        var workflow = model.Workflow;
        var identity = workflow.Identity;
        var definitionId = identity.DefinitionId;

        // Get a workflow draft version.
        var draft = !string.IsNullOrWhiteSpace(definitionId)
            ? await workflowPublisher.GetDraftAsync(definitionId, cancellationToken)
            : default;

        var isNew = draft == null;

        // Create a new workflow in case no existing definition was found.
        if (draft == null)
        {
            draft = workflowPublisher.New();

            if (!string.IsNullOrWhiteSpace(definitionId))
                draft = draft.WithDefinitionId(definitionId);
        }

        // Update the draft with the received model.
        draft = draft with { Root = workflow.Root, Triggers = workflow.Triggers };

        // Publish?
        if (model.Publish)
            draft = await workflowPublisher.PublishAsync(draft, cancellationToken);
        else
            draft = await workflowPublisher.SaveDraftAsync(draft, cancellationToken);

        var statusCode = isNew ? StatusCodes.Status201Created : StatusCodes.Status200OK;

        return Results.Json(draft, serializerOptions, statusCode: statusCode);
    }
}