using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Containers;
using Elsa.Activities.Http;
using Elsa.Expressions;
using Elsa.Runtime.Models;
using HttpMethods = Microsoft.AspNetCore.Http.HttpMethods;

namespace Elsa.Samples.Web1.Workflows
{
    public static class HttpWorkflow
    {
        public static Workflow Create() =>
            new()
            {
                Id = "HttpWorkflow",
                Root = new Sequence(
                    new HttpEndpoint
                    {
                        Path = new Literal<string>("/hello-world"),
                        SupportedMethods = new Literal<ICollection<string>>(new[] { HttpMethods.Get })
                    },
                    new HttpResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = "Hello World!"
                    })
            };
    }
}