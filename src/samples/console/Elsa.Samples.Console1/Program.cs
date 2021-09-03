using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Activities.Console;
using Elsa.Contracts;
using Elsa.Pipelines.ActivityExecution.Components;
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

            var invoker = services.GetRequiredService<IActivityInvoker>();
            var workflow1 = new Func<IActivity>(HelloWorldWorkflow.Create);
            var workflow2 = new Func<IActivity>(HelloGoodbyeWorkflow.Create);
            var workflow3 = new Func<IActivity>(GreetingWorkflow.Create);
            var workflow4 = new Func<IActivity>(ConditionalWorkflow.Create);
            var workflow5 = new Func<IActivity>(ForWorkflow.Create);
            var workflow6 = new Func<IActivity>(BlockingWorkflow.Create);
            var workflow7 = new Func<IActivity>(ForkedWorkflow.Create);
            var workflow8 = new Func<IActivity>(DynamicActivityWorkflow.Create);
            var workflow9 = new Func<IActivity>(CustomizedActivityWorkflow.Create);

            var workflowFactory = workflow7;
            var workflowGraph = workflowFactory();
            var workflowExecutionResult = await invoker.InvokeAsync(workflowGraph);
            var workflowState = workflowExecutionResult.WorkflowState;

            if (workflowState.Bookmarks.Any())
            {
                Console.WriteLine("Press enter to resume workflow.");
                Console.ReadLine();

                workflowGraph = workflowFactory();
                foreach (var bookmarkState in workflowState.Bookmarks)
                {
                    await invoker.ResumeAsync(bookmarkState.Name, workflowGraph, workflowState);
                }
            }
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection()
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Warning))
                .AddElsa()
                .AddActivityDriver<MyWriteLineDriver>("MyWriteLine")
                .AddActivityDriver<WriteLineDriver, CustomWriteLine>();

            return services.BuildServiceProvider();
        }
    }
}