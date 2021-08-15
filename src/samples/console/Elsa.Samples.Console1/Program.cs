using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Pipelines.NodeExecution.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elsa.Samples.Console1
{
    class Program
    {
        static async Task Main()
        {
            var services = CreateServices().ConfigureNodeExecutionPipeline(pipeline => pipeline
                .UseLogging()
                .UseNodeDrivers());

            var invoker = services.GetRequiredService<INodeInvoker>();
            var workflow1 = CreateHelloWorldWorkflow();
            var workflow2 = CreateGreetingWorkflow();
            await invoker.InvokeAsync(workflow1);
            await invoker.InvokeAsync(workflow2);
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection()
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddElsa()
                .AddExpressionHandler<LiteralHandler>(typeof(Literal<>))
                .AddExpressionHandler<DelegateHandler>(typeof(Delegate<>))
                .AddNodeDriver<SequenceDriver>()
                .AddNodeDriver<WriteLineDriver>()
                .AddNodeDriver<ReadLineDriver>();

            return services.BuildServiceProvider();
        }

        private static INode CreateHelloWorldWorkflow() =>
            new Sequence
            {
                Nodes = new[]
                {
                    new WriteLine("Hello World!"),
                    new WriteLine("Goodbye cruel world...")
                }
            };

        private static INode CreateGreetingWorkflow()
        {
            var readLine1 = new ReadLine();

            return new Sequence
            {
                Nodes = new INode[]
                {
                    new WriteLine("What's your name?"),
                    readLine1,
                    new WriteLine(() => $"Nice to meet you, {readLine1.Output}!")
                }
            };
        }
    }
}