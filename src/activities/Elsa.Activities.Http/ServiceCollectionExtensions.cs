using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Activities.Http
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpActivities(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor()
                .AddActivityDriver<HttpEndpointDriver>()
                .AddActivityDriver<HttpResponseDriver>();
        }
    }
}