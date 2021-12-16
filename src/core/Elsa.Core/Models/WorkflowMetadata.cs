namespace Elsa.Models;

public record WorkflowMetadata(WorkflowIdentity Identity, WorkflowPublication Publication, string? Name = default)
{
    public static WorkflowMetadata VersionOne => new(WorkflowIdentity.VersionOne, WorkflowPublication.LatestAndPublished);
}