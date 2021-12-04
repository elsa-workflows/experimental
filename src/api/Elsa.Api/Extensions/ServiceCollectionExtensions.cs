using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Providers;
using Elsa.Api.Core.Services;
using Elsa.Api.HostedServices;
using Elsa.Api.Serializers;
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
                .AddSingleton<IActivityRegistry, ActivityRegistry>()
                .AddSingleton<IActivityRegistryPopulator, ActivityRegistryPopulator>()
                .AddSingleton<IActivityPropertyDefaultValueResolver, ActivityPropertyDefaultValueResolver>()
                .AddSingleton<IActivityPropertyOptionsResolver, ActivityPropertyOptionsResolver>()
                .AddSingleton<IActivityProvider, TypedActivityProvider>()
                .AddSingleton<IWellKnownTypeRegistry, WellKnownTypeRegistry>()
                .AddSingleton<IActivityFactory, ActivityFactory>()
                .AddSingleton<IExpressionSyntaxRegistry, ExpressionSyntaxRegistry>()
                .AddSingleton<IExpressionSyntaxProvider, DefaultExpressionSyntaxProvider>()
                .AddSingleton<IExpressionSyntaxRegistryPopulator, ExpressionSyntaxRegistryPopulator>()
                .AddSingleton<WorkflowSerializerOptionsProvider>()
                .AddHostedService<RegisterActivityDescriptors>()
                .AddHostedService<RegisterExpressionSyntaxDescriptors>();
        }
    }
}