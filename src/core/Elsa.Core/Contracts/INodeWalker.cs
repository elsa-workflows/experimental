using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeWalker
    {
        GraphNode Walk(INode node);
    }
}