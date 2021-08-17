using Elsa.Contracts;

namespace Elsa.Models
{
    public record ScheduledNode(INode Node, INode? Owner = default);
}