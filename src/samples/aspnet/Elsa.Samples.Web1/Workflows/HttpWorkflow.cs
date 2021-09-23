using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Contracts;
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
            // Create triggers.
            var httpTrigger = new HttpTrigger
            {
                Path = new Input<string>("/hello-world"),
                SupportedMethods = new Input<ICollection<string>>(new[] { HttpMethods.Get })
            };

            // Register triggers.
            builder.Triggers = new List<ITrigger>
            {
                httpTrigger
            };

            // Setup workflow graph.
            builder.Root = new Sequence
            {
                Activities =
                {
                    new If
                    {
                        Condition = new Input<bool>(true),
                        Then = new Sequence(
                            new WriteLine("It's true!"),
                            new HttpTrigger
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