using System.Collections.Generic;
using System.Net.Http;
using Elsa.Attributes;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Activities.Http
{
    public class HttpEndpoint : Activity, ITrigger
    {
        [Input] public Input<string> Path { get; set; } = default!;
        [Input] public Input<ICollection<string>> SupportedMethods { get; set; } = new(new[] { HttpMethod.Get.Method });
        [Output] public Output<HttpRequestModel>? Result { get; set; }
    }
}