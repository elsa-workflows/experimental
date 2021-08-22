using System.Collections.Generic;
using System.Net;
using Elsa.Activities.Containers;
using Elsa.Activities.Http;
using Elsa.Contracts;
using Elsa.Expressions;

namespace Elsa.Samples.Web1.Workflows
{
    using HttpMethods = Microsoft.AspNetCore.Http.HttpMethods;

    public static class HttpWorkflow
    {
        public static IActivity Create() =>
            new Sequence(
                new HttpEndpoint
                {
                    Path = new Literal<string>("/hello-world"),
                    SupportedMethods = new Literal<ICollection<string>>(new[] { HttpMethods.Get })
                },
                new Elsa.Activities.Http.HttpResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "Hello World!"
                });
    }
}