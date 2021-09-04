using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Expressions;
using Elsa.Runtime.Models;
using Elsa.WorkflowBuilderApi.Contracts;

namespace Elsa.Samples.Web1.Workflows
{
    using HttpMethods = Microsoft.AspNetCore.Http.HttpMethods;

    public class HttpWorkflowBuilder : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            // Create triggers.
            var httpEndpoint = new HttpEndpoint
            {
                Path = new Literal<string>("/hello-world"),
                SupportedMethods = new Literal<ICollection<string>>(new[] { HttpMethods.Get })
            };
            
            // Register triggers.
            builder.Triggers.Add(new Trigger(httpEndpoint));
            
            // Setup workflow graph.
            builder.Root = new Sequence(
                httpEndpoint,
                new If
                {
                    //Condition = new Delegate<bool>(context => httpEndpoint.)
                },
                new Elsa.Activities.Http.HttpResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "Hello World!"
                });
        }
    }
}