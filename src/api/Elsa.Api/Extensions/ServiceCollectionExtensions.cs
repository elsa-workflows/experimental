using Elsa.Api.HostedServices;
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
            .AddHostedService<RegisterDescriptors>()
            .AddHostedService<RegisterExpressionSyntaxDescriptors>();
    }
}