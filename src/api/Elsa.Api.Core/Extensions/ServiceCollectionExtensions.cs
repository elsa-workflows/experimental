using System;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Options;
using Elsa.Api.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, Action<ApiOptions> configure) => services.Configure(configure);
        public static IServiceCollection AddActivity<T>(this IServiceCollection services) => services.ConfigureApiOptions(options => options.ActivityTypes.Add(typeof(T)));
    }
}