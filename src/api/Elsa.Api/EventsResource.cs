using System;
using Elsa.Api.Endpoints.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Elsa.Api
{
    public class EventsResource
    {
        private readonly IEndpointRouteBuilder _endpoints;

        public EventsResource(IEndpointRouteBuilder endpoints)
        {
            _endpoints = endpoints;
        }

        public EventsResource MapTrigger()
        {
            _endpoints.MapPost("api/events/{eventName}/trigger", Trigger.Handle);
            return this;
        }
    }
    
    public static class EventsResourceExtensions
    {
        public static void MapEventsResource(this IEndpointRouteBuilder endpoints, Action<EventsResource> configureResource) => configureResource(new(endpoints));
    }
}