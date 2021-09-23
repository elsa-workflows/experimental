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

    public class ForkedHttpWorkflow : IWorkflow
    {
        public string Id => nameof(ForkedHttpWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder builder)
        {
            // Create triggers.
            var httpEndpoint = new HttpTrigger
            {
                Path = new Input<string>("/fork"),
                SupportedMethods = new Input<ICollection<string>>(new[] { HttpMethods.Get })
            };

            // Register triggers.
            builder.Triggers = new List<ITrigger>
            {
                httpEndpoint
            };

            // Setup workflow graph.
            builder.Root = new Sequence(
                new Fork
                {
                    Branches =
                    {
                        new Sequence
                        {
                            Activities =
                            {
                                new HttpTrigger { Path = new Input<string>("/fork/branch-1"), },
                                new WriteLine("Branch 1 continues!")
                            }
                        },
                        new Sequence
                        {
                            Activities =
                            {
                                new HttpTrigger { Path = new Input<string>("/fork/branch-2"), },
                                new WriteLine("Branch 2 continues!")
                            }
                        },
                    }
                },
                new HttpResponse
                {
                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
                    Content = new Input<string?>("Done!")
                });
        }
    }
}