using System;
using Elsa.Activities.Http.Extensions;
using Elsa.Api;
using Elsa.Persistence.InMemory.Extensions;
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
    .AddHttpActivities()
    .ConfigureWorkflowRuntime(options =>
    {
        options.Workflows.Add("HelloWorldWorkflow", new HelloWorldWorkflow());
        options.Workflows.Add("HttpWorkflow", new HttpWorkflow());
    });

// Configure middleware pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", (Delegate)(() => "Hello World!"));
app.MapWorkflows(workflows => workflows.MapExecute());
app.UseHttpActivities();

// Run.
app.Run();