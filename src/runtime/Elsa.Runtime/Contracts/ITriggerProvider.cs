using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface ITriggerProvider
    {
        bool GetSupportsActivity(object activity);
        ValueTask<IEnumerable<object>> GetHashInputsAsync(TriggerIndexingContext context, CancellationToken cancellationToken = default);
    }
}