using System;
using Elsa.Contracts;
using Elsa.Pipelines.WorkflowExecution.Components;

namespace Elsa.Pipelines.WorkflowExecution
{
    public class WorkflowExecutionPipeline : IWorkflowExecutionPipeline
    {
        private readonly IServiceProvider _serviceProvider;
        private WorkflowMiddlewareDelegate? _pipeline;
        
        public WorkflowExecutionPipeline(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
        public WorkflowMiddlewareDelegate Pipeline => _pipeline ??= CreateDefaultPipeline();

        public WorkflowMiddlewareDelegate Setup(Action<IWorkflowExecutionBuilder> setup)
        {
            var builder = new WorkflowExecutionPipelineBuilder(_serviceProvider);
            setup(builder);
            _pipeline = builder.Build();
            return _pipeline;
        }

        private WorkflowMiddlewareDelegate CreateDefaultPipeline() => Setup(x => x
            .UseActivityScheduler()
        );
    }
}