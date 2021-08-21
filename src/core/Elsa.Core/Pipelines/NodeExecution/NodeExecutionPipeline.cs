using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Pipelines.NodeExecution.Components;

namespace Elsa.Pipelines.NodeExecution
{
    public class NodeExecutionPipeline : INodeExecutionPipeline
    {
        private readonly IServiceProvider _serviceProvider;
        private NodeMiddlewareDelegate? _pipeline;

        public NodeExecutionPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public NodeMiddlewareDelegate Setup(Action<INodeExecutionBuilder> setup)
        {
            var builder = new NodeExecutionBuilder(_serviceProvider);
            setup(builder);
            _pipeline = builder.Build();
            return _pipeline;
        }

        public async Task ExecuteAsync(NodeExecutionContext context)
        {
            var pipeline = _pipeline ?? CreateDefaultPipeline();
            
            await pipeline(context);
        }

        private NodeMiddlewareDelegate CreateDefaultPipeline() => Setup(x => x
            .UseNodeDrivers()
        );
    }
}