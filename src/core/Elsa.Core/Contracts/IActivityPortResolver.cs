using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface IActivityPortResolver
    {
        bool GetSupportsActivity(IActivity activity);
        IEnumerable<IActivity> GetPorts(IActivity activity);
    }
}