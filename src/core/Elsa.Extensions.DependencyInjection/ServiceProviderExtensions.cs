using System;
using Elsa.Contracts;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static IServiceProvider ConfigureDefaultActivityExecutionPipeline(this IServiceProvider services, Action<IActivityExecutionBuilder> setup)
    {
        var pipeline = services.GetRequiredService<IActivityExecutionPipeline>();
        pipeline.Setup(setup);
        return services;
    }
        
    public static IServiceProvider ConfigureDefaultWorkflowExecutionPipeline(this IServiceProvider services, Action<IWorkflowExecutionBuilder> setup)
    {
        var pipeline = services.GetRequiredService<IWorkflowExecutionPipeline>();
        pipeline.Setup(setup);
        return services;
    }
}