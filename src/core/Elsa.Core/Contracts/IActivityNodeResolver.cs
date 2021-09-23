using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface IActivityNodeResolver
    {
        int Priority { get; }
        bool GetSupportsActivity(IActivity activity);
        IEnumerable<IActivity> GetNodes(IActivity activity);
    }
}