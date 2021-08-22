using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityWalker
    {
        Node Walk(IActivity activity);
    }
}