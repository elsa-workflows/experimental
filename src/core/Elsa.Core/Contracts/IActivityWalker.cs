using Elsa.Models;

namespace Elsa.Contracts
{
    /// <summary>
    /// Walks an activity tree starting at the root.
    /// </summary>
    public interface IActivityWalker
    {
        Node Walk(IActivity activity);
    }
}