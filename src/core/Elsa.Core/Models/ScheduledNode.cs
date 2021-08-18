using Elsa.Contracts;

namespace Elsa.Models
{
    public record ScheduledNode(INode Node, NodeCompletionCallback? CompletionCallback = default);
}