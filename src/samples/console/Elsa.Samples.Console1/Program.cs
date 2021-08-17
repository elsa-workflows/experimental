using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Nodes.ControlFlow;
using Elsa.Nodes.Primitives;
using Elsa.Pipelines.NodeExecution.Components;
using Elsa.Samples.Console1.Workflows;
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
            var workflow1 = HelloWorldWorkflow.Create();
            var workflow2 = GreetingWorkflow.Create();
            var workflow3 = ConditionalWorkflow.Create();
            var workflow4 = ForEachWorkflow.Create();
            var workflow5 = BlockingWorkflow.Create();
            var workflow6 = ForkedWorkflow.Create();
            var workflowExecutionContext = await invoker.InvokeAsync(workflow5);
            var workflowStateService = services.GetRequiredService<IWorkflowStateService>();
            var workflowState = workflowStateService.CreateState(workflowExecutionContext);

            foreach (var bookmark in workflowState.Bookmarks)
            {
                
            }
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
                .AddNodeDriver<ForDriver>()
                .AddNodeDriver<EventDriver>()
                .AddNodeDriver<ForkDriver>();

            return services.BuildServiceProvider();
        }
    }
}