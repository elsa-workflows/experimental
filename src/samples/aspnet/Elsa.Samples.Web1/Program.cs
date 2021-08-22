using System;
using Elsa.Activities.Http;
using Elsa.Samples.Web1.Workflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services
    .AddElsa()
    .AddHttpActivities()
    .ConfigureWorkflowRuntime(options => { options.WorkflowFactories.Add(HttpWorkflow.Create); });

// Configure middleware pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", (Delegate)(() => "Hello World!"));

// Run.
app.Run();