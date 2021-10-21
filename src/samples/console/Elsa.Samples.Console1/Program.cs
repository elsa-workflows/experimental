using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Middleware.WorkflowExecution;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Pipelines.ActivityExecution.Components;
using Elsa.Pipelines.WorkflowExecution.Components;
using Elsa.Samples.Console1.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elsa.Samples.Console1
{
    class Program
    {
        static async Task Main()
        {
            var serviceProvider = CreateServices();

            serviceProvider
                .ConfigureDefaultActivityExecutionPipeline(pipeline => pipeline
                    .UseLogging()
                    .UseActivityDrivers()
                )
                .ConfigureDefaultWorkflowExecutionPipeline(pipeline => pipeline
                    .PersistWorkflows()
                    .UseActivityScheduler()
                );

            var workflow1 = new Func<IActivity>(HelloWorldWorkflow.Create);
            var workflow2 = new Func<IActivity>(HelloGoodbyeWorkflow.Create);
            var workflow3 = new Func<IActivity>(GreetingWorkflow.Create);
            var workflow4 = new Func<IActivity>(ConditionalWorkflow.Create);
            var workflow5 = new Func<IActivity>(ForWorkflow.Create);
            var workflow6 = new Func<IActivity>(BlockingWorkflow.Create);
            var workflow7 = new Func<IActivity>(ForkedWorkflow.Create);
            var workflow8 = new Func<IActivity>(CustomizedActivityWorkflow.Create);
            var workflow9 = new Func<IActivity>(VariablesWorkflow.Create);
            var workflow10 = new Func<IActivity>(WhileWorkflow.Create);
            var workflow11 = new Func<IActivity>(ForEachWorkflow.Create);
            var workflow12 = new Func<IActivity>(ParallelForEachWorkflow.Create);
            var workflow13 = new Func<IActivity>(BlockingParallelForEachWorkflow.Create);

            var workflowFactory = workflow13;
            var workflowGraph = workflowFactory();
            var workflow = new Workflow(workflowGraph);

            var workflowDefinition = new WorkflowDefinition
            {
                Workflow = workflow,
            };

            var workflowEngine = serviceProvider.GetRequiredService<IWorkflowEngine>();
            var workflowExecutionResult = await workflowEngine.ExecuteAsync(workflowDefinition);
            var workflowState = workflowExecutionResult.WorkflowState;
            var bookmarks = new List<Bookmark>(workflowExecutionResult.Bookmarks);

            while (bookmarks.Any())
            {
                workflow = workflow with { Root = workflowFactory() };
                foreach (var bookmark in bookmarks.ToList())
                {
                    Console.WriteLine("Press enter to resume workflow with bookmark {0}.", new
                    {
                        bookmark.Id,
                        bookmark.Name,
                        bookmark.Hash,
                        bookmark.ActivityId,
                        bookmark.ActivityInstanceId,
                        bookmark.CallbackMethodName
                    });

                    Console.ReadLine();

                    var resumeResult = await workflowEngine.ExecuteAsync(workflowDefinition, workflowState, bookmark);
                    workflowState = resumeResult.WorkflowState;
                    bookmarks = resumeResult.Bookmarks.ToList();
                }
            }
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection();

            services
                .AddElsa()
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Warning))
                .AddInMemoryWorkflowInstanceStore()
                .AddInMemoryBookmarkStore()
                .AddInMemoryTriggerStore();

            return services.BuildServiceProvider();
        }
    }
}