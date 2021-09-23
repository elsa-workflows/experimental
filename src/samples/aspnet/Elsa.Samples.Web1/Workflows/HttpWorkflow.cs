using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Samples.Web1.Workflows
{
    using HttpMethods = Microsoft.AspNetCore.Http.HttpMethods;
    using HttpResponse = Elsa.Activities.Http.HttpResponse;

    public class HttpWorkflow : IWorkflow
    {
        public string Id => nameof(HttpWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder builder)
        {
            // Variables.
            var pathVariable = new Variable<string>("/hello-world");

            // Create triggers.
            var httpEndpoint = new HttpEndpoint
            {
                Path = new Input<string>(pathVariable),
                SupportedMethods = new Input<ICollection<string>>(new[] { HttpMethods.Get })
            };

            // Register triggers.
            builder.Triggers.Add(new TriggerSource(httpEndpoint));

            // Setup workflow graph.
            builder.Root = new Sequence
            {
                Variables = { pathVariable },
                Activities =
                {
                    httpEndpoint,
                    new If
                    {
                        Condition = new Input<bool>(true),
                        Then = new Sequence(
                            new WriteLine("It's true!"),
                            new HttpEndpoint
                            {
                                Path = new Input<string>("/hello-world/true"),
                                SupportedMethods = new Input<ICollection<string>>(new[] { HttpMethods.Post })
                            },
                            new WriteLine("Let's continue")
                        ),
                        Else = new WriteLine("It's not true!")
                    },
                    new HttpResponse
                    {
                        StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
                        Content = new Input<string?>("Hello World!")
                    }
                }
            };
        }
    }
}