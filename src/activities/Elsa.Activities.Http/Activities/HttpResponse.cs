using System.Net;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Services;
using Microsoft.AspNetCore.Http;

namespace Elsa.Activities.Http
{
    public class HttpResponse : Activity
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Content { get; set; }
    }

    public class HttpResponseDriver : ActivityDriver<HttpResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpResponseDriver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async ValueTask ExecuteAsync(HttpResponse activity, ActivityExecutionContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var response = httpContext.Response;

            response.StatusCode = (int)activity.StatusCode;

            if (activity.Content != null)
                await response.WriteAsync(activity.Content, context.CancellationToken);
        }
    }
}