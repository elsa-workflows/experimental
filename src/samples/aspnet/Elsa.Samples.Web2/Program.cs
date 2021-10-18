using System;
using Elsa.Activities.Http.Extensions;
using Elsa.Api;
using Elsa.Persistence.Abstractions.Middleware.WorkflowExecution;
using Elsa.Persistence.InMemory.Extensions;
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
    .AddHttpWorkflowServices()
    .AddProtoActorWorkflowHost()
    .ConfigureWorkflowRuntime(options => { options.Workflows.Add("HelloWorldWorkflow", new HelloWorldWorkflow()); });

// Configure middleware pipeline.
var app = builder.Build();

app.Services.ConfigureDefaultWorkflowExecutionPipeline(pipeline => pipeline
    .PersistWorkflows()
    .UseActivityScheduler()
);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", (Delegate)(() => "Hello World!"));

// Map Elsa API endpoints.
app.MapWorkflowsEndpoint(workflows => workflows.MapExecute().MapDispatch());
app.MapEventsEndpoint(events => events.MapTrigger());

// Register Elsa HTTP activity middleware.
app.UseHttpActivities();

// Run.
app.Run();