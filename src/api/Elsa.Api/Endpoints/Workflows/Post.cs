using System.Threading;
using System.Threading.Tasks;
using Elsa.Management.Contracts;
using Elsa.Management.Mappers;
using Elsa.Management.Serialization;
using Elsa.Models;
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
        var definition = !string.IsNullOrWhiteSpace(definitionId)
            ? await workflowPublisher.GetDraftAsync(definitionId, cancellationToken)
            : default;

        var isNew = definition == null;

        // Create a new workflow in case no existing definition was found.
        if (definition == null)
        {
            definition = workflowPublisher.New();

            if (!string.IsNullOrWhiteSpace(definitionId))
                definition.DefinitionId = definitionId;
        }

        // Update the definition with the received model. 
        definition.Root = workflow.Root;
        definition.Triggers = workflow.Triggers;

        // Publish?
        if (model.Publish)
            definition = await workflowPublisher.PublishAsync(definition, cancellationToken);
        else
            definition = await workflowPublisher.SaveDraftAsync(definition, cancellationToken);

        // Synchronize workflow model with updated definition.
        workflow = mapper.Map(definition);

        var statusCode = isNew ? StatusCodes.Status201Created : StatusCodes.Status200OK;

        return Results.Json(workflow, serializerOptions, statusCode: statusCode);
    }
}