using System;
using Elsa.Activities.Http.Extensions;
using Elsa.Api;
using Elsa.Persistence.Abstractions.Middleware.WorkflowExecution;
using Elsa.Persistence.InMemory.Extensions;
using Elsa.Pipelines.WorkflowExecution.Components;
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
    .AddHttpWorkflowServices()
    .ConfigureWorkflowRuntime(options =>
    {
        options.Workflows.Add("HelloWorldWorkflow", new HelloWorldWorkflow());
        options.Workflows.Add("HttpWorkflow", new HttpWorkflow());
        options.Workflows.Add("ForkedHttpWorkflow", new ForkedHttpWorkflow());
    });

// Configure middleware pipeline.
var app = builder.Build();

app.Services.ConfigureDefaultWorkflowExecutionPipeline(pipeline => pipeline
    .PersistWorkflows()
    .UseActivityScheduler()
);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", (Delegate)(() => "Hello World!"));
app.MapWorkflows(workflows => workflows.MapExecute());
app.UseHttpActivities();

// Run.
app.Run();