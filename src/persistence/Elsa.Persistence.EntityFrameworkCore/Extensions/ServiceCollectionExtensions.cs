using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Persistence.EntityFrameworkCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkCorePersistence(IServiceCollection services)
    {
        return services;
    }
}