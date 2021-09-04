using System.Collections.Generic;
using System.Net.Http;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Http
{
    public class HttpEndpoint : CodeActivity
    {
        public IExpression<string> Path { get; set; } = default!;
        public IExpression<ICollection<string>> SupportedMethods { get; set; } = new Literal<ICollection<string>>(new[] { HttpMethod.Get.Method });
    }

    public class HttpEndpointDriver : ActivityDriver<HttpEndpoint>
    {
        protected override void Execute(HttpEndpoint activity, ActivityExecutionContext context)
        {
            //var input = context.WorkflowExecutionContext
        }
    }
}