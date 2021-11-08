using Elsa.Api.Core.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.ActivityDescriptors
{
    public static class List
    {
        public static IResult HandleAsync(IActivityDescriptorRegistry registry)
        {
            var descriptors = registry.ListDescriptors();
            return Results.Ok(descriptors);
        }
    }
}