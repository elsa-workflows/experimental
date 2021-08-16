using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface INodeWalker
    {
        IEnumerable<INode> Walk(INode node);
    }
}