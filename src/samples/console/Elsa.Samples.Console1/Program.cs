using System;
using System.Linq;
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
            var workflow1 = new Func<INode>(HelloWorldWorkflow.Create);
            var workflow2 = new Func<INode>(HelloGoodbyeWorkflow.Create);
            var workflow3 = new Func<INode>(GreetingWorkflow.Create);
            var workflow4 = new Func<INode>(ConditionalWorkflow.Create);
            var workflow5 = new Func<INode>(ForEachWorkflow.Create);
            var workflow6 = new Func<INode>(BlockingWorkflow.Create);
            var workflow7 = new Func<INode>(ForkedWorkflow.Create);
            
            var workflowFactory = workflow6;
            var workflowExecutionContext = await invoker.InvokeAsync(workflowFactory());
            var workflowStateService = services.GetRequiredService<IWorkflowStateService>();
            var workflowState = workflowStateService.CreateState(workflowExecutionContext);

            if (workflowState.Bookmarks.Any())
            {
                Console.WriteLine("Press enter to resume workflow.");
                Console.ReadLine();

                var workflow = workflowFactory();
                foreach (var bookmarkState in workflowState.Bookmarks)
                {
                    await invoker.ResumeAsync(bookmarkState.Name, workflow, workflowState);
                }
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