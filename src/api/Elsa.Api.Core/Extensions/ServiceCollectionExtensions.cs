using System;
using Elsa.Api.Core.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, Action<ApiOptions> configure)
        {
            return services.Configure(configure);
        }
        
        public static IServiceCollection AddActivity<T>(this IServiceCollection services)
        {
            return services.Configure(configure);
        }
    }
}