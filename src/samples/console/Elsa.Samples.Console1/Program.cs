﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Contracts;
using Elsa.Expressions;
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

            var workflowFactory = workflow5;
            var workflowGraph = workflowFactory();
            var workflowExecutionContext = await invoker.InvokeAsync(workflowGraph);
            var workflowStateService = services.GetRequiredService<IWorkflowStateService>();
            var workflowState = workflowStateService.CreateState(workflowExecutionContext);

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
                .AddExpressionHandler<LiteralHandler>(typeof(Literal<>))
                .AddExpressionHandler<DelegateHandler>(typeof(Delegate<>))
                .AddNodeDriver<SequenceDriver>()
                .AddNodeDriver<WriteLineDriver>()
                .AddNodeDriver<ReadLineDriver>()
                .AddNodeDriver<IfDriver>()
                .AddNodeDriver<ForDriver>()
                .AddNodeDriver<EventDriver>()
                .AddNodeDriver<ForkDriver>()
                .AddNodeDriver<MyWriteLineDriver>("MyWriteLine")
                .AddNodeDriver<WriteLineDriver, CustomWriteLine>();

            return services.BuildServiceProvider();
        }
    }
}