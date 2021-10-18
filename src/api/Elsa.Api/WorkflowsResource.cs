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
            _endpoints.MapPost("api/workflows/{id}/execute", Execute.HandleAsync);
            return this;
        }
        
        public WorkflowsResource MapDispatch()
        {
            _endpoints.MapPost("api/workflows/{id}/dispatch", Dispatch.HandleAsync);
            return this;
        }
    }
    
    public static class WorkflowsResourceExtensions
    {
        public static void MapWorkflowsEndpoint(this IEndpointRouteBuilder endpoints, Action<WorkflowsResource> configureResource) => configureResource(new(endpoints));
    }
}