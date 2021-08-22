using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Activities.Http
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpActivities(this IServiceCollection services)
        {
            return services
                .AddActivityDriver<HttpEndpointDriver>()
                .AddActivityDriver<HttpResponseDriver>();
        }
    }
}