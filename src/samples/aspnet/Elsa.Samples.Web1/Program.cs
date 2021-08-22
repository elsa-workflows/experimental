using System;
using Elsa.Activities.Http;
using Elsa.Api;
using Elsa.Api.Endpoints.Workflows;
using Elsa.Samples.Web1.Workflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services
    .AddElsa()
    .AddHttpActivities()
    .ConfigureWorkflowRuntime(options => { options.WorkflowFactories.Add("HttpWorkflow", HttpWorkflow.Create); });

// Configure middleware pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", (Delegate)(() => "Hello World!"));
app.MapWorkflows(workflows => workflows.MapExecute());

// Run.
app.Run();