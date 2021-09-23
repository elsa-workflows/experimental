using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.Activities.Http
{
    public class HttpEndpointTriggerProvider : ITriggerProvider
    {
        public bool GetSupportsActivity(object activity) => activity is HttpEndpoint;

        public ValueTask<IEnumerable<object>> GetHashInputsAsync(TriggerIndexingContext context, CancellationToken cancellationToken = default)
        {
            var httpEndpoint = (HttpEndpoint)context.Activity;
            var path = context.ExpressionExecutionContext.Get(httpEndpoint.Path);
            var methods = context.ExpressionExecutionContext.Get(httpEndpoint.SupportedMethods);
            var hashInputs = methods!.Select(x => (path!.ToLowerInvariant(), x.ToLowerInvariant())).Cast<object>().ToArray();

            return ValueTask.FromResult<IEnumerable<object>>(hashInputs);
        }
    }
}