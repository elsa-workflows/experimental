using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Providers;
using Elsa.Api.Core.Services;
using Elsa.Api.HostedServices;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services used by Elsa API endpoints.
        /// </summary>
        public static IServiceCollection AddWorkflowApiServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IActivityDescriber, ActivityDescriber>()
                .AddSingleton<IActivityDescriptorRegistry, ActivityDescriptorRegistry>()
                .AddSingleton<IActivityDescriptorRegistryPopulator, ActivityDescriptorRegistryPopulator>()
                .AddSingleton<IActivityPropertyDefaultValueResolver, ActivityPropertyDefaultValueResolver>()
                .AddSingleton<IActivityPropertyOptionsResolver, ActivityPropertyOptionsResolver>()
                .AddSingleton<IActivityDescriptorProvider, TypedActivityDescriptorProvider>()
                .AddSingleton<IWellKnownTypeRegistry, WellKnownTypeRegistry>()
                .AddHostedService<RegisterActivityDescriptors>();
        }
    }
}