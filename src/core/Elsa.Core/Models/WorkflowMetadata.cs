namespace Elsa.Models;

public record WorkflowMetadata(WorkflowIdentity Identity, WorkflowPublication Publication)
{
    public static WorkflowMetadata VersionOne => new(WorkflowIdentity.VersionOne, WorkflowPublication.LatestAndPublished);
}