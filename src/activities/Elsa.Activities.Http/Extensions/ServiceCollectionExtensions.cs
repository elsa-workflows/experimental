using Elsa.Activities.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpWorkflowServices(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor()
                .AddTriggerProvider<HttpEndpointTriggerProvider>();
        }
    }
}