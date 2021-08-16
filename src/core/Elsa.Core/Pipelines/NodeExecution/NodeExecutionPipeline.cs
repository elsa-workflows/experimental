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
        private ExecuteNode? _pipeline;

        public NodeExecutionPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ExecuteNode Setup(Action<INodeExecutionBuilder> setup)
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

        private ExecuteNode CreateDefaultPipeline() => Setup(x => x
            .UseNodeDrivers()
        );
    }
}