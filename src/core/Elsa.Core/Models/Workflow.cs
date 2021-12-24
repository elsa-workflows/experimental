using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models;

public record Workflow(
    WorkflowIdentity Identity,
    WorkflowPublication Publication,
    WorkflowMetadata Metadata,
    IActivity Root,
    ICollection<ITrigger> Triggers)
{
    public static Workflow FromActivity(IActivity root) => new(WorkflowIdentity.VersionOne, WorkflowPublication.LatestAndUnpublished, new WorkflowMetadata(), root, new List<ITrigger>());
}