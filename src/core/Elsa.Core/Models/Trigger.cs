using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.Models
{
    public class Trigger : Activity, ITrigger
    {
        public virtual ValueTask<IEnumerable<object>> GetHashInputsAsync(TriggerIndexingContext context, CancellationToken cancellationToken = default)
        {
            var hashes = GetHashInputs(context);
            return ValueTask.FromResult<IEnumerable<object>>(hashes);
        }

        protected virtual IEnumerable<object> GetHashInputs(TriggerIndexingContext context) => Enumerable.Empty<object>();
    }
}