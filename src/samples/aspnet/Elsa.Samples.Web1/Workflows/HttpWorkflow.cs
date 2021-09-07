using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Samples.Web1.Workflows
{
    using HttpMethods = Microsoft.AspNetCore.Http.HttpMethods;

    public class HttpWorkflow : IWorkflow
    {
        public string Id => nameof(HttpWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder builder)
        {
            // Create triggers.
            var httpEndpoint = new HttpEndpoint
            {
                Path = new Literal<string>("/hello-world"),
                SupportedMethods = new Literal<ICollection<string>>(new[] { HttpMethods.Get })
            };

            // Register triggers.
            builder.Triggers.Add(new TriggerSource(httpEndpoint));

            // Setup workflow graph.
            builder.Root = new Sequence(
                httpEndpoint,
                new If
                {
                    Condition = new Literal<bool>(true),
                    Then = new Sequence(
                        new WriteLine("It's true!"),
                        new HttpEndpoint
                        {
                            Path = new Literal<string>("/hello-world/true"),
                            SupportedMethods = new Literal<ICollection<string>>(new[] { HttpMethods.Post })
                        },
                        new WriteLine("Let's continue")
                    ),
                    Else = new WriteLine("It's not true!")
                },
                new Elsa.Activities.Http.HttpResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "Hello World!"
                });
        }
    }
}