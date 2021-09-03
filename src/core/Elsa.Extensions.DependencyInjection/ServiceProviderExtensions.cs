using System;
using Elsa.Contracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider ConfigureNodeExecutionPipeline(this IServiceProvider services, Action<IActivityExecutionBuilder> setup)
        {
            var pipeline = services.GetRequiredService<IActivityExecutionPipeline>();
            pipeline.Setup(setup);
            return services;
        }
    }
}