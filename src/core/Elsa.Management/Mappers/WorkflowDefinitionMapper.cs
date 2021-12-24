using Elsa.Models;
using Elsa.Persistence.Entities;

namespace Elsa.Management.Mappers;

public class WorkflowDefinitionMapper
{
    public Workflow Map(WorkflowDefinition definition)
    {
        return new Workflow(
            new WorkflowIdentity(definition.DefinitionId, definition.Version, definition.Id),
            new WorkflowPublication(definition.IsLatest, definition.IsPublished),
            new WorkflowMetadata(definition.Name, definition.CreatedAt),
            definition.Root,
            definition.Triggers);
    }

    public WorkflowDefinition Map(Workflow workflow) => Map(workflow, new WorkflowDefinition());

    public WorkflowDefinition Map(Workflow workflow, WorkflowDefinition definition)
    {
        var identity = workflow.Identity;
        var publication = workflow.Publication;
        var metadata = workflow.Metadata;

        definition.Id = identity.Id;
        definition.DefinitionId = identity.DefinitionId;
        definition.Version = identity.Version;
        definition.Name = metadata.Name;
        definition.Root = workflow.Root;
        definition.Triggers = workflow.Triggers;
        definition.CreatedAt = metadata.CreatedAt;
        definition.IsLatest = publication.IsLatest;
        definition.IsPublished = publication.IsPublished;

        return definition;
    }
}