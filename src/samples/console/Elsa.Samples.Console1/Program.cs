using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elsa.Samples.Console1
{
    class Program
    {
        static async Task Main()
        {
            var services = CreateServices();
            var invoker = services.GetRequiredService<INodeInvoker>();
            //var workflow = CreateHelloWorldWorkflow();
            var workflow = CreateGreetingWorkflow();
            await invoker.InvokeAsync(workflow);
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection()
                .AddLogging(logging => logging.AddConsole())
                .AddElsa()
                .AddExpressionHandler<LiteralHandler>(typeof(Literal<>))
                .AddExpressionHandler<DelegateHandler>(typeof(Delegate<>))
                .AddNodeDriver<SequenceDriver>()
                .AddNodeDriver<WriteLineDriver>()
                .AddNodeDriver<ReadLineDriver>();

            // services.Configure<WorkflowEngineOptions>(elsa => elsa
            //     .RegisterExpressionHandler<LiteralHandler>(typeof(Literal<>))
            //     .RegisterExpressionHandler<DelegateHandler>(typeof(Delegate<>))
            //     .RegisterNodeDriver<Sequence, SequenceDriver>()
            //     .RegisterNodeDriver<WriteLine, WriteLineDriver>()
            //     .RegisterNodeDriver<ReadLine, ReadLineDriver>());

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