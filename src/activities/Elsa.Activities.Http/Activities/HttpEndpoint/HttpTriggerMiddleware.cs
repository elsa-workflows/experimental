using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Microsoft.AspNetCore.Http;

namespace Elsa.Activities.Http
{
    public class HttpTriggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHasher _hasher;

        public HttpTriggerMiddleware(RequestDelegate next, IHasher hasher)
        {
            _next = next;
            _hasher = hasher;
        }

        public async Task InvokeAsync(HttpContext httpContext, IStimulusInterpreter stimulusInterpreter, IWorkflowInstructionExecutor instructionExecutor)
        {
            var path = GetPath(httpContext);
            var method = httpContext.Request.Method!.ToLowerInvariant();
            var abortToken = httpContext.RequestAborted;
            var hash = _hasher.Hash((path.ToLowerInvariant(), method.ToLowerInvariant()));
            var activityTypeName = nameof(HttpTrigger);
            var stimulus = Stimuli.Standard(activityTypeName, hash);
            var instructions = await stimulusInterpreter.GetExecutionInstructionsAsync(stimulus, abortToken);
            var executionResults = (await instructionExecutor.ExecuteInstructionsAsync(instructions, CancellationToken.None)).ToList();

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
                var model = new
                {
                    workflowInstanceIds = executionResults.Where(x => x != null).Select(x => x!.WorkflowExecutionResult.WorkflowState.Id).ToArray()
                };
                var json = JsonSerializer.Serialize(model);
                await response.WriteAsync(json, abortToken);
            }
        }

        private string GetPath(HttpContext httpContext) => httpContext.Request.Path.Value.ToLowerInvariant();
    }
}