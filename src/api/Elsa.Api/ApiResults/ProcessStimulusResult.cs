using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.ApiResults
{
    public class ProcessStimulusResult : IResult
    {
        public ProcessStimulusResult(IStimulus stimulus, ExecuteInstructionOptions options)
        {
            Stimulus = stimulus;
            Options = options;
        }

        public IStimulus Stimulus { get; }
        public ExecuteInstructionOptions Options { get; }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var response = httpContext.Response;
            var stimulusInterpreter = httpContext.RequestServices.GetRequiredService<IStimulusInterpreter>();
            var abortToken = httpContext.RequestAborted;
            var instructions = await stimulusInterpreter.GetInstructionsAsync(Stimulus, abortToken);
            var instructionExecutor = httpContext.RequestServices.GetRequiredService<IWorkflowInstructionExecutor>();
            var executionResults = (await instructionExecutor.ExecuteAsync(instructions, Options, CancellationToken.None)).ToList();

            if (!response.HasStarted)
            {
                var model = new
                {
                    Results = executionResults
                };

                await response.WriteAsJsonAsync(model, httpContext.RequestAborted);
            }
        }
    }
}