using System.IO;
using System.Reflection;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.Http;
using Elsa.Activities.Timers;
using Elsa.Dsl.Abstractions;
using Elsa.Dsl.Contracts;
using Elsa.Dsl.Extensions;
using Elsa.Models;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Runtime.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var workflowEngine = CreateWorkflowEngine();
var serviceProvider = workflowEngine.ServiceProvider;
var typeSystem = serviceProvider.GetRequiredService<ITypeSystem>();
var dslEngine = serviceProvider.GetRequiredService<IDslEngine>();
var functionActivityRegistry = serviceProvider.GetRequiredService<IFunctionActivityRegistry>();

typeSystem.Register<int>("int");
typeSystem.Register<float>("float");
typeSystem.Register<string>("string");
typeSystem.Register("Variable<>", typeof(Variable<>));
typeSystem.Register<Sequence>();
typeSystem.Register<ReadLine>();
typeSystem.Register<WriteLine>();
typeSystem.Register<HttpTrigger>();
typeSystem.Register<TimerTrigger>();

functionActivityRegistry.RegisterFunction("print", nameof(WriteLine), new[] { nameof(WriteLine.Text) });
functionActivityRegistry.RegisterFunction("read", nameof(ReadLine), new[]{ nameof(ReadLine.Output) });

var assembly = Assembly.GetExecutingAssembly();
var resource = assembly.GetManifestResourceStream("Elsa.Samples.Console2.Sample1.elsa");

var script = await new StreamReader(resource!).ReadToEndAsync();
var workflowDefinition = dslEngine.Parse(script);

await workflowEngine.ExecuteWorkflowAsync(workflowDefinition);

IWorkflowEngine CreateWorkflowEngine()
{
    var builder = DefaultWorkflowEngineBuilder.CreateDefaultBuilder();

    builder.Services
        .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Warning))
        .AddInMemoryWorkflowInstanceStore()
        .AddInMemoryBookmarkStore()
        .AddInMemoryTriggerStore();

    return builder.BuildWorkflowEngine();
}