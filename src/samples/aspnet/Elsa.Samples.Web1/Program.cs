using System;
using Elsa.Activities.Http.Extensions;
using Elsa.Api;
using Elsa.Api.Endpoints.ActivityDescriptors;
using Elsa.Api.Endpoints.Events;
using Elsa.Api.Endpoints.Workflows;
using Elsa.Persistence.Abstractions.Middleware.WorkflowExecution;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Pipelines.WorkflowExecution.Components;
using Elsa.Runtime.ProtoActor.Extensions;
using Elsa.Samples.Web1.Workflows;
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
    .ConfigureWorkflowRuntime(options =>
    {
        options.Workflows.Add("HelloWorldWorkflow", new HelloWorldWorkflow());
        options.Workflows.Add("HttpWorkflow", new HttpWorkflow());
        options.Workflows.Add("ForkedHttpWorkflow", new ForkedHttpWorkflow());
        options.Workflows.Add(nameof(CompositeActivitiesWorkflow), new CompositeActivitiesWorkflow());
    });

// Configure middleware pipeline.
var app = builder.Build();

// Configure workflow engine execution pipeline.
app.Services.ConfigureDefaultWorkflowExecutionPipeline(pipeline => pipeline
    .PersistWorkflows()
    .UseActivityScheduler()
);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", (Delegate)(() => "Hello World!"));

// Map Elsa API endpoints.
app.MapWorkflowsResource(resource => resource.Execute());
app.MapEventsResource(resource => resource.Trigger());
app.MapActivityDescriptorsResource(resource => resource.List());

// Register Elsa HTTP activity middleware.
app.UseHttpActivities();

// Run.
app.Run();