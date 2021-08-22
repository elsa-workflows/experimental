using System;
using Elsa.Api.Endpoints.Workflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Elsa.Api
{
    public class WorkflowsResource
    {
        private readonly IEndpointRouteBuilder _endpoints;

        public WorkflowsResource(IEndpointRouteBuilder endpoints)
        {
            _endpoints = endpoints;
        }

        public WorkflowsResource MapExecute()
        {
            _endpoints.MapPost("api/workflows/{id}", Execute.HandleAsync);
            return this;
        }
    }
    
    public static class WorkflowsResourceExtensions
    {
        public static void MapWorkflows(this IEndpointRouteBuilder endpoints, Action<WorkflowsResource> configureResource) => configureResource(new(endpoints));
    }
}