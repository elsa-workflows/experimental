using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models;

public record Workflow(WorkflowMetadata Metadata, IActivity Root, ICollection<ITrigger> Triggers)
{
    public static Workflow FromActivity(IActivity root) => new(WorkflowMetadata.VersionOne, root, new List<ITrigger>());
}