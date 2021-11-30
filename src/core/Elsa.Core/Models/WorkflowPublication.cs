namespace Elsa.Models;

public record WorkflowPublication(bool IsLatest, bool IsPublished)
{
    public static WorkflowPublication LatestAndPublished => new(true, true);
}