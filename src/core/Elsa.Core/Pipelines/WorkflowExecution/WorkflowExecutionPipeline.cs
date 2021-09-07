using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Pipelines.ActivityExecution;
using Elsa.Pipelines.ActivityExecution.Components;
using Elsa.Pipelines.WorkflowExecution.Components;

namespace Elsa.Pipelines.WorkflowExecution
{
    public class WorkflowExecutionPipeline : IWorkflowExecutionPipeline
    {
        private readonly IServiceProvider _serviceProvider;
        private WorkflowMiddlewareDelegate? _pipeline;

        public WorkflowExecutionPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public WorkflowMiddlewareDelegate Setup(Action<IWorkflowExecutionBuilder> setup)
        {
            var builder = new WorkflowExecutionBuilder(_serviceProvider);
            setup(builder);
            _pipeline = builder.Build();
            return _pipeline;
        }

        public async Task ExecuteAsync(WorkflowExecutionContext context)
        {
            var pipeline = _pipeline ?? CreateDefaultPipeline();
            
            await pipeline(context);
        }

        private WorkflowMiddlewareDelegate CreateDefaultPipeline() => Setup(x => x
            .UseStackScheduler()
        );
    }
}