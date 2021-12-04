using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Models;

public class Trigger : Activity, ITrigger
{
    public virtual ValueTask<IEnumerable<object>> GetHashInputsAsync(TriggerIndexingContext context, CancellationToken cancellationToken = default)
    {
        var hashes = GetHashInputs(context);
        return ValueTask.FromResult(hashes);
    }

    protected virtual IEnumerable<object> GetHashInputs(TriggerIndexingContext context) => Enumerable.Empty<object>();
}