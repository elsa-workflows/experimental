using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Elsa.Api.Endpoints.Workflows
{
    public class WorkflowsResource
    {
        private readonly IEndpointRouteBuilder _endpoints;

        public WorkflowsResource(IEndpointRouteBuilder endpoints)
        {
            _endpoints = endpoints;
        }

        public WorkflowsResource Post()
        {
            _endpoints.MapPost("api/workflows", Workflows.PostAsync);
            return this;
        }

        public WorkflowsResource Execute()
        {
            _endpoints.MapPost("api/workflows/{id}/execute", Workflows.ExecuteAsync);
            return this;
        }

        public WorkflowsResource Dispatch()
        {
            _endpoints.MapPost("api/workflows/{id}/dispatch", Workflows.DispatchAsync);
            return this;
        }
    }

    public static class WorkflowsResourceExtensions
    {
        public static void MapWorkflows(this IEndpointRouteBuilder endpoints, Action<WorkflowsResource> configureResource) => configureResource(new(endpoints));
        public static void MapWorkflows(this IEndpointRouteBuilder endpoints) => endpoints.MapWorkflows(x => x.Post().Execute().Dispatch());
    }
}