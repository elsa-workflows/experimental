using Microsoft.AspNetCore.Builder;

namespace Elsa.Activities.Http.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpActivities(this IApplicationBuilder app) => app.UseMiddleware<HttpEndpointMiddleware>();
    }
}