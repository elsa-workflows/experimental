using Elsa.Services;

namespace Elsa.Contracts
{
    public interface INodeWalker
    {
        GraphNode Walk(INode node);
    }
}