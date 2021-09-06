using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Http
{
    public class HttpEndpointDriver : ActivityDriver<HttpEndpoint>
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
    }
}