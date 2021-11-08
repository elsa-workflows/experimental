using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Services;
using Elsa.Api.HostedServices;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElsaApiServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IActivityDescriptorRegistryPopulator, ActivityDescriptorRegistryPopulator>()
                .AddHostedService<RegisterActivityDescriptors>();
        }
    }
}