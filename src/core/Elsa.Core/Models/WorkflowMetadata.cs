namespace Elsa.Models;

public record WorkflowMetadata(WorkflowIdentity Identity, WorkflowPublication Publication)
{
    public static WorkflowMetadata VersionOne => new WorkflowMetadata(WorkflowIdentity.VersionOne, WorkflowPublication.LatestAndPublished);
}