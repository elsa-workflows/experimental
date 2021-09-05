using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Services;

namespace Elsa.Runtime.Services
{
    public abstract class TriggerDriver<TActivity> : ActivityDriver<TActivity>, ITriggerProvider where TActivity : new()
    {
        ValueTask<IEnumerable<object>> ITriggerProvider.GetHashInputsAsync(object activity, CancellationToken cancellationToken) => GetHashInputsAsync((TActivity)activity, cancellationToken);
        protected virtual ValueTask<IEnumerable<object>> GetHashInputsAsync(TActivity activity, CancellationToken cancellationToken = default) => new(Array.Empty<object>());
    }
}