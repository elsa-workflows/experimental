using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Activities.Http
{
    public class HttpEndpointTriggerProvider : ITriggerProvider
    {
        private readonly IExpressionEvaluator _expressionEvaluator;

        public HttpEndpointTriggerProvider(IExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }

        public bool GetSupportsActivity(object activity) => activity is HttpEndpoint;

        public async ValueTask<IEnumerable<object>> GetHashInputsAsync(object activity, CancellationToken cancellationToken = default)
        {
            var httpEndpoint = (HttpEndpoint)activity;
            var path = await _expressionEvaluator.EvaluateAsync(httpEndpoint.Path, new ExpressionExecutionContext());
            var methods = await _expressionEvaluator.EvaluateAsync(httpEndpoint.SupportedMethods, new ExpressionExecutionContext());
            var hashInputs = methods.Select(x => (path.ToLowerInvariant(), x.ToLowerInvariant())).Cast<object>().ToArray();

            return hashInputs;
        }
    }
}