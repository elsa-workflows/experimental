using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Nodes.ControlFlow;
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
                //.UseLogging()
                .UseNodeDrivers()
            );

            var invoker = services.GetRequiredService<INodeInvoker>();
            var workflow1 = CreateHelloWorldWorkflow();
            var workflow2 = CreateGreetingWorkflow();
            var workflow3 = CreateConditionalWorkflow();
            var workflow4 = CreateForEachWorkflow();
            await invoker.InvokeAsync(workflow4);
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection()
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Warning))
                .AddElsa()
                .AddExpressionHandler<LiteralHandler>(typeof(Literal<>))
                .AddExpressionHandler<DelegateHandler>(typeof(Delegate<>))
                .AddNodeDriver<SequenceDriver>()
                .AddNodeDriver<WriteLineDriver>()
                .AddNodeDriver<ReadLineDriver>()
                .AddNodeDriver<IfDriver>()
                .AddNodeDriver<ForDriver>();

            return services.BuildServiceProvider();
        }

        private static INode CreateHelloWorldWorkflow() =>
            new Sequence
            {
                Nodes = new INode[]
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
        
        private static INode CreateConditionalWorkflow()
        {
            var readLine1 = new ReadLine();

            return new Sequence
            {
                Nodes = new INode[]
                {
                    new WriteLine("What's your age?"),
                    readLine1,
                    new If
                    {
                        Condition = new Delegate<bool>(() => int.Parse(readLine1.Output!) >= 16),
                        True = new Sequence
                        {
                            Nodes = new INode[]
                            {
                                new WriteLine("Enjoy your driver's license!"),
                                new WriteLine("But be careful!")
                            }
                        },
                        False = new WriteLine("Enjoy your bicycle!")
                    }
                }
            };
        }
        
        private static INode CreateForEachWorkflow()
        {
            var for1 = new For
            {
                Start = 1,
                End = 3,
                Next = new WriteLine("Done.")
            };

            for1.Iterate = new WriteLine(() => for1.CurrentValue.ToString());
            
            return new Sequence
            {
                Nodes = new INode[]
                {
                    new WriteLine("Counting numbers from 1 to 10:"),
                    for1
                }
            };
        }
    }
}