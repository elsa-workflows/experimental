using Elsa.Contracts;

namespace Elsa.Models
{
    public abstract class Node : INode
    {
        public string? Name { get; set; }
    }
}