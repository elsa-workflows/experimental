using System;
using Elsa.Api.Core.Options;
using Elsa.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, Action<ApiOptions> configure) => services.Configure(configure);
    public static IServiceCollection AddActivity<T>(this IServiceCollection services) where T:IActivity => services.ConfigureApiOptions(options => options.ActivityTypes.Add(typeof(T)));
    public static IServiceCollection AddTrigger<T>(this IServiceCollection services) where T:ITrigger => services.ConfigureApiOptions(options => options.TriggerTypes.Add(typeof(T)));
}