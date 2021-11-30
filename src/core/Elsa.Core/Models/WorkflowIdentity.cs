namespace Elsa.Models;

public record WorkflowIdentity(string Id, int Version)
{
    public static WorkflowIdentity VersionOne => new("1", 1);
}