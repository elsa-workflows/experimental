using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Elsa.Api.Endpoints.ActivityDescriptors
{
    public class ActivityDescriptorsResource
    {
        private readonly IEndpointRouteBuilder _endpoints;

        public ActivityDescriptorsResource(IEndpointRouteBuilder endpoints)
        {
            _endpoints = endpoints;
        }

        public ActivityDescriptorsResource List()
        {
            _endpoints.MapGet("api/activity-descriptors", Elsa.Api.Endpoints.ActivityDescriptors.List.HandleAsync);
            return this;
        }
    }
    
    public static class ActivityDescriptorsResourceExtensions
    {
        public static void MapActivityDescriptorsResource(this IEndpointRouteBuilder endpoints, Action<ActivityDescriptorsResource> configureResource) => configureResource(new(endpoints));
    }
}