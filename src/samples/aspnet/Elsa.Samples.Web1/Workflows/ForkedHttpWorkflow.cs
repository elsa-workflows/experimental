using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Samples.Web1.Workflows
{
    public class ForkedHttpWorkflow : IWorkflow
    {
        public string Id => nameof(ForkedHttpWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder builder)
        {
            // Create triggers.
            var httpTrigger = new HttpTrigger
            {
                Path = new Input<string>("/fork"),
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
                    new Fork
                    {
                        JoinMode = new Input<JoinMode>(JoinMode.WaitAny),
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
                            }
                        },
                        Next = new WriteHttpResponse
                        {
                            StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
                            Content = new Input<string?>("Done!")
                        }
                    }
                }
            };
        }
    }
}