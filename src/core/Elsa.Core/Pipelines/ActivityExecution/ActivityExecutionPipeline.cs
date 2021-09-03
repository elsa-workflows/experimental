using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Pipelines.ActivityExecution.Components;

namespace Elsa.Pipelines.ActivityExecution
{
    public class ActivityExecutionPipeline : IActivityExecutionPipeline
    {
        private readonly IServiceProvider _serviceProvider;
        private ActivityMiddlewareDelegate? _pipeline;

        public ActivityExecutionPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ActivityMiddlewareDelegate Setup(Action<IActivityExecutionBuilder> setup)
        {
            var builder = new ActivityExecutionBuilder(_serviceProvider);
            setup(builder);
            _pipeline = builder.Build();
            return _pipeline;
        }

        public async Task ExecuteAsync(ActivityExecutionContext context)
        {
            var pipeline = _pipeline ?? CreateDefaultPipeline();
            
            await pipeline(context);
        }

        private ActivityMiddlewareDelegate CreateDefaultPipeline() => Setup(x => x
            .UseNodeDrivers()
        );
    }
}