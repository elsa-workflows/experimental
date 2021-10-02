using System.IO;
using System.Reflection;
using Elsa.Activities.Http;
using Elsa.Contracts;
using Elsa.Dsl.Contracts;
using Elsa.Extensions;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Runtime.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var workflowEngine = CreateWorkflowEngine();
var serviceProvider = workflowEngine.ServiceProvider;
var triggerTypeRegistry = serviceProvider.GetRequiredService<ITriggerTypeRegistry>();
var dslEngine = serviceProvider.GetRequiredService<IDslEngine>();

triggerTypeRegistry.Register<HttpTrigger>();

var assembly = Assembly.GetExecutingAssembly();
var resource = assembly.GetManifestResourceStream("Elsa.Samples.Console2.Sample1.elsa");

var script = await new StreamReader(resource!).ReadToEndAsync();
var workflow = dslEngine.Parse(script);

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