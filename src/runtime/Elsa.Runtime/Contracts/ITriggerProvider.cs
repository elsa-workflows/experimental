using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface ITriggerProvider
    {
        ValueTask<IEnumerable<object>> GetHashInputsAsync(object activity, CancellationToken cancellationToken = default);
    }
}