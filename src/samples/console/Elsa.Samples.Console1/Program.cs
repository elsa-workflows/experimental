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
using Elsa.Runtime.Contracts;
using Elsa.Samples.Console1.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elsa.Samples.Console1
{
    class Program
    {
        static async Task Main()
        {
            var workflowEngine = CreateWorkflowEngine();

            workflowEngine.ServiceProvider
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

            var workflowFactory = workflow2;
            var workflowGraph = workflowFactory();
            var workflow = new Workflow("MyWorkflow", 1, DateTime.Now, workflowGraph);
            var workflowExecutionResult = await workflowEngine.ExecuteWorkflowAsync(workflow);
            var workflowState = workflowExecutionResult.WorkflowState;
            var bookmarks = new List<Bookmark>(workflowExecutionResult.Bookmarks);

            while (bookmarks.Any())
            {
                Console.WriteLine("Press enter to resume workflow.");
                Console.ReadLine();

                workflow = workflow with { Root = workflowFactory() };
                foreach (var bookmark in bookmarks.ToList())
                {
                    bookmarks.Remove(bookmark);
                    var resumeResult = await workflowEngine.ResumeAsync(workflow, bookmark, workflowState);
                    bookmarks.AddRange(resumeResult.Bookmarks);
                }
            }
        }

        private static IWorkflowEngine CreateWorkflowEngine()
        {
            var builder = DefaultWorkflowEngineBuilder.CreateDefaultBuilder();

            builder.Services
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Warning))
                .AddInMemoryWorkflowInstanceStore()
                .AddInMemoryBookmarkStore()
                .AddInMemoryTriggerStore();

            return builder.BuildWorkflowEngine();
        }
    }
}