using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Activities.Http.Middleware
{
    public class HttpEndpointMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHasher _hasher;

        public HttpEndpointMiddleware(RequestDelegate next, IHasher hasher)
        {
            _next = next;
            _hasher = hasher;
        }

        public async Task InvokeAsync(HttpContext httpContext, IWorkflowManager workflowManager)
        {
            var path = GetPath(httpContext);
            var method = httpContext.Request.Method!.ToLowerInvariant();
            var abortToken = httpContext.RequestAborted;
            var hash = _hasher.Hash((path.ToLowerInvariant(), method.ToLowerInvariant()));
            var bookmarkName = nameof(HttpEndpoint);
            var results = (await workflowManager.ResumeBookmarksAsync(bookmarkName, hash, CancellationToken.None)).ToList();

            if (!results.Any())
            {
                await _next(httpContext);
                return;
            }

            var response = httpContext.Response;

            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status200OK;
                var json = JsonSerializer.Serialize(results);
                await response.WriteAsync(json, abortToken);
            }
        }

        private string GetPath(HttpContext httpContext) => httpContext.Request.Path.Value.ToLowerInvariant();
    }
}