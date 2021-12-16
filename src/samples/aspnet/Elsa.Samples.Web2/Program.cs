using Elsa.Activities.Http.Extensions;
using Elsa.Api.Endpoints.Events;
using Elsa.Api.Endpoints.Workflows;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Persistence.Middleware.WorkflowExecution;
using Elsa.Pipelines.WorkflowExecution.Components;
using Elsa.Runtime.ProtoActor.Extensions;
using Elsa.Samples.Web2.Workflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services.

builder.Services
    .AddElsa()
    .AddInMemoryWorkflowInstanceStore()
    .AddInMemoryBookmarkStore()
    .AddInMemoryTriggerStore()
    .IndexWorkflowTriggers()
    .AddHttpActivityServices()
    .AddProtoActorWorkflowHost()
    .ConfigureWorkflowRuntime(options => { options.Workflows.Add("HelloWorldWorkflow", new HelloWorldWorkflow()); });

// Configure middleware pipeline.
var app = builder.Build();

// Configure workflow engine execution pipeline.
app.Services.ConfigureDefaultWorkflowExecutionPipeline(pipeline => pipeline
    .PersistWorkflows()
    .UseActivityScheduler()
);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", () => "Hello World!");

// Map Elsa API endpoints.
app.MapWorkflows(workflows => workflows
    .Execute()
    .Dispatch());

app.MapEvents(events => events
    .Trigger());

// Register Elsa HTTP activity middleware.
app.UseHttpActivities();

// Run.
app.Run();