using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Services;

namespace Elsa.Activities.Http
{
    public class HttpEndpoint : CodeActivity, ITrigger
    {
        public IExpression<string> Path { get; set; } = default!;
        public IExpression<ICollection<string>> SupportedMethods { get; set; } = new Literal<ICollection<string>>(new[] { HttpMethod.Get.Method });
    }

    public class HttpEndpointDriver : TriggerDriver<HttpEndpoint>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;

        public HttpEndpointDriver(IExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }
        
        protected override void Execute(HttpEndpoint activity, ActivityExecutionContext context)
        {
            //var input = context.WorkflowExecutionContext
        }

        protected override async ValueTask<IEnumerable<object>> GetHashInputsAsync(HttpEndpoint activity, CancellationToken cancellationToken = default)
        {
            var path = await _expressionEvaluator.EvaluateAsync(activity.Path, new ExpressionExecutionContext());
            var methods = await _expressionEvaluator.EvaluateAsync(activity.SupportedMethods, new ExpressionExecutionContext());
            var hashInputs = methods.Select(x => (path, x)).Cast<object>().ToArray();

            return hashInputs;
        }
    }
}