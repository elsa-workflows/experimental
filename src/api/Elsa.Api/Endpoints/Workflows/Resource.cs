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

        public WorkflowsResource Execute()
        {
            _endpoints.MapPost("api/workflows/{id}/execute", Workflows.Execute.HandleAsync);
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
        public static void MapWorkflowsResource(this IEndpointRouteBuilder endpoints, Action<WorkflowsResource> configureResource) => configureResource(new(endpoints));
    }
}