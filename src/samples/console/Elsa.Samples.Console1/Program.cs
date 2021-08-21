using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
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
            var workflow2 = HelloGoodbyeWorkflow.Create();
            var workflow3 = GreetingWorkflow.Create();
            var workflow4 = ConditionalWorkflow.Create();
            var workflow5 = ForEachWorkflow.Create();
            var workflow6 = BlockingWorkflow.Create();
            var workflow7 = ForkedWorkflow.Create();
            var workflowExecutionContext = await invoker.InvokeAsync(workflow6);
            var workflowStateService = services.GetRequiredService<IWorkflowStateService>();
            var workflowState = workflowStateService.CreateState(workflowExecutionContext);
            var nodeDriverRegistry = services.GetRequiredService<INodeDriverRegistry>();
            var identityGraphService = services.GetRequiredService<IIdentityGraphService>();
            var identityGraph = identityGraphService.CreateIdentityGraph(workflowExecutionContext.Root).ToList();

            if (workflowState.Bookmarks.Any())
            {
                Console.WriteLine("Press enter to resume workflow.");
                Console.ReadLine();

                foreach (var bookmarkState in workflowState.Bookmarks)
                {
                    var scheduledNodeState = bookmarkState.ScheduledNode;
                    var blockingNode = identityGraph.First(x => x.NodeName == scheduledNodeState.NodeId);
                    var resumeActionName = bookmarkState.ResumeActionName;
                    var node = blockingNode.Node.Node;

                    // Setup Resume delegate.
                    var driver = nodeDriverRegistry.GetDriver(node);
                    var driverType = driver!.GetType();
                    var resumeMethodInfo = resumeActionName != null ? driverType.GetMethod(resumeActionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null;
                    var resumeDelegate = resumeMethodInfo != null ? (ExecuteNodeDelegate)Delegate.CreateDelegate(typeof(ExecuteNodeDelegate), driver, resumeMethodInfo) : Noop;

                    // Setup Completion delegate
                    var scheduledNode = new ScheduledNode(node);

                    var bookmark = new Bookmark(scheduledNode, bookmarkState.Name, bookmarkState.Data, resumeDelegate);
                    await invoker.ResumeAsync(bookmark, workflowExecutionContext.Root);
                }
            }
        }

        private static ValueTask Noop(NodeExecutionContext context) => new();

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