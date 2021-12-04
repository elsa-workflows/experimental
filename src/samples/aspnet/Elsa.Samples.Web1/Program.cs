using Elsa.Activities.Console;
using Elsa.Activities.Http.Extensions;
using Elsa.Api.Core.Extensions;
using Elsa.Api.Endpoints.ActivityDescriptors;
using Elsa.Api.Endpoints.Events;
using Elsa.Api.Endpoints.Workflows;
using Elsa.Api.Extensions;
using Elsa.Persistence.Abstractions.Middleware.WorkflowExecution;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Pipelines.WorkflowExecution.Components;
using Elsa.Runtime.ProtoActor.Extensions;
using Elsa.Samples.Web1.Activities;
using Elsa.Samples.Web1.Workflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services.
services
    .AddElsa()
    .AddInMemoryWorkflowInstanceStore()
    .AddInMemoryBookmarkStore()
    .AddInMemoryTriggerStore()
    .IndexWorkflowTriggers()
    .AddHttpActivityServices()
    .AddWorkflowApiServices()
    .AddProtoActorWorkflowHost()
    .ConfigureWorkflowRuntime(options =>
    {
        options.Workflows.Add("HelloWorldWorkflow", new HelloWorldWorkflow());
        options.Workflows.Add("HttpWorkflow", new HttpWorkflow());
        options.Workflows.Add("ForkedHttpWorkflow", new ForkedHttpWorkflow());
        options.Workflows.Add(nameof(CompositeActivitiesWorkflow), new CompositeActivitiesWorkflow());
    });

// Testing only: allow client app to connect from anywhere.
services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));


// Register available activities.
services.AddActivity<WriteLine>();
services.AddActivity<WriteLines>();
services.AddActivity<ReadLine>();

// Configure middleware pipeline.
var app = builder.Build();
var serviceProvider = app.Services;

// Configure workflow engine execution pipeline.
serviceProvider.ConfigureDefaultWorkflowExecutionPipeline(pipeline => pipeline
    .PersistWorkflows()
    .UseActivityScheduler()
);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseCors();
app.MapGet("/", () => "Hello World!");

// Map Elsa API endpoints.
app.MapWorkflows();
app.MapEvents();
app.MapActivityDescriptors();

// Register Elsa HTTP activity middleware.
app.UseHttpActivities();

// Run.
app.Run();