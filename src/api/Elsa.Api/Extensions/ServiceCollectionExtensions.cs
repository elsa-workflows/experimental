using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Providers;
using Elsa.Api.Core.Services;
using Elsa.Api.HostedServices;
using Elsa.Api.Serializers;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Extensions;

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
            .AddSingleton<IPropertyDefaultValueResolver, PropertyDefaultValueResolver>()
            .AddSingleton<IPropertyOptionsResolver, PropertyOptionsResolver>()
            .AddSingleton<IActivityProvider, TypedActivityProvider>()
            .AddSingleton<IActivityFactory, ActivityFactory>()
            .AddSingleton<ITriggerDescriber, TriggerDescriber>()
            .AddSingleton<ITriggerRegistry, TriggerRegistry>()
            .AddSingleton<ITriggerRegistryPopulator, TriggerRegistryPopulator>()
            .AddSingleton<ITriggerFactory, TriggerFactory>()
            .AddSingleton<ITriggerProvider, TypedTriggerProvider>()
            .AddSingleton<IWellKnownTypeRegistry, WellKnownTypeRegistry>()
            
            .AddSingleton<IExpressionSyntaxRegistry, ExpressionSyntaxRegistry>()
            .AddSingleton<IExpressionSyntaxProvider, DefaultExpressionSyntaxProvider>()
            .AddSingleton<IExpressionSyntaxRegistryPopulator, ExpressionSyntaxRegistryPopulator>()
            .AddSingleton<WorkflowSerializerOptionsProvider>()
            .AddHostedService<RegisterDescriptors>()
            .AddHostedService<RegisterExpressionSyntaxDescriptors>();
    }
}