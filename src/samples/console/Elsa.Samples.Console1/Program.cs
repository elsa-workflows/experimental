using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Activities.Console;
using Elsa.Contracts;
using Elsa.Models;
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
                .UseActivityDrivers()
            );

            var invoker = services.GetRequiredService<IWorkflowInvoker>();
            var workflow1 = new Func<IActivity>(HelloWorldWorkflow.Create);
            var workflow2 = new Func<IActivity>(HelloGoodbyeWorkflow.Create);
            var workflow3 = new Func<IActivity>(GreetingWorkflow.Create);
            var workflow4 = new Func<IActivity>(ConditionalWorkflow.Create);
            var workflow5 = new Func<IActivity>(ForWorkflow.Create);
            var workflow6 = new Func<IActivity>(BlockingWorkflow.Create);
            var workflow7 = new Func<IActivity>(ForkedWorkflow.Create);
            var workflow8 = new Func<IActivity>(DynamicActivityWorkflow.Create);
            var workflow9 = new Func<IActivity>(CustomizedActivityWorkflow.Create);

            var workflowFactory = workflow5;
            var workflowGraph = workflowFactory();
            var workflow = new Workflow("MyWorkflow", 1, DateTime.Now, workflowGraph, new List<TriggerSource>());
            var workflowExecutionResult = await invoker.InvokeAsync(workflow);
            var workflowState = workflowExecutionResult.WorkflowState;
            var bookmarks = workflowExecutionResult.Bookmarks;

            if (bookmarks.Any())
            {
                Console.WriteLine("Press enter to resume workflow.");
                Console.ReadLine();

                workflow = workflow with { Root = workflowFactory() };
                foreach (var bookmark in bookmarks)
                    await invoker.ResumeAsync(workflow, bookmark, workflowState);
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