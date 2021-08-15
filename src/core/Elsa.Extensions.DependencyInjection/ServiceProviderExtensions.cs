using System;
using Elsa.Contracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider ConfigureNodeExecutionPipeline(this IServiceProvider services, Action<INodeExecutionBuilder> setup)
        {
            var pipeline = services.GetRequiredService<INodeExecutionPipeline>();
            pipeline.Setup(setup);
            return services;
        } 
    }
}