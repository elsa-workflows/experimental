using System.Threading;
using System.Threading.Tasks;
using Elsa.Management.Contracts;
using Elsa.Management.Serialization;
using Elsa.Models;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.Workflows;

public static partial class Workflows
{
    public record SaveWorkflowRequest(Workflow Workflow, bool Publish);

    public static async Task<IResult> PostAsync(HttpContext httpContext, WorkflowSerializerOptionsProvider serializerOptionsProvider, IWorkflowPublisher workflowPublisher, CancellationToken cancellationToken)
    {
        var serializerOptions = serializerOptionsProvider.CreateSerializerOptions();
        var model = (await httpContext.Request.ReadFromJsonAsync<SaveWorkflowRequest>(serializerOptions, cancellationToken))!;
        var workflow = model.Workflow;
        var metadata = workflow.Metadata;
        var definitionId = metadata.Identity.Id;
        var definition = !string.IsNullOrWhiteSpace(definitionId) ? await workflowPublisher.GetDraftAsync(definitionId, cancellationToken) : default;
        var isNew = definition == null;

        if (definition == null)
        {
            definition = workflowPublisher.New();

            if (!string.IsNullOrWhiteSpace(definitionId))
                definition.DefinitionId = definitionId;
        }

        definition.Root = workflow.Root;
        definition.Triggers = workflow.Triggers;

        if (model.Publish)
            await workflowPublisher.PublishAsync(definition, cancellationToken);
        else
            await workflowPublisher.SaveDraftAsync(definition, cancellationToken);

        // Synchronize workflow model with updated definition.
        workflow = workflow with
        {
            Metadata = new WorkflowMetadata(
                new WorkflowIdentity(definition.DefinitionId, definition.Version), 
                new WorkflowPublication(definition.IsLatest, definition.IsPublished), workflow.Metadata.Name)
        }; 
        
        var statusCode = isNew ? StatusCodes.Status201Created : StatusCodes.Status200OK;

        return Results.Json(workflow, serializerOptions, statusCode: statusCode);
    }
}