using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Microsoft.AspNetCore.Http;

namespace Elsa.Activities.Http
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

        public async Task InvokeAsync(HttpContext httpContext, IWorkflowInstructionManager workflowInstructionManager, IWorkflowRegistry workflowRegistry, IWorkflowTriggerStore workflowTriggerStore, IWorkflowBookmarkStore workflowBookmarkStore)
        {
            var path = GetPath(httpContext);
            var method = httpContext.Request.Method!.ToLowerInvariant();
            var abortToken = httpContext.RequestAborted;
            var hash = _hasher.Hash((path.ToLowerInvariant(), method.ToLowerInvariant()));
            var activityTypeName = nameof(HttpEndpoint);
            var stimulus = Stimuli.Standard(activityTypeName, hash);
            var instructions = await workflowInstructionManager.GetExecutionInstructionsAsync(stimulus, abortToken);
            var executionResults = (await workflowInstructionManager.ExecuteInstructionsAsync(instructions, CancellationToken.None)).ToList();
            
            if (!executionResults.Any())
            {
                await _next(httpContext);
                return;
            }

            var response = httpContext.Response;

            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status200OK;
                var json = "{}";
                await response.WriteAsync(json, abortToken);
            }
        }

        private string GetPath(HttpContext httpContext) => httpContext.Request.Path.Value.ToLowerInvariant();
    }
}