using System.Text.Json;
using Elsa.Management.Serialization.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Management.Serialization;

public class WorkflowSerializerOptionsProvider
{
    private readonly IServiceProvider _serviceProvider;
    public WorkflowSerializerOptionsProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public JsonSerializerOptions CreateSerializerOptions() => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            Create<TypeJsonConverter>(),
            Create<ActivityJsonConverterFactory>(),
            Create<TriggerJsonConverterFactory>(),
            Create<FlowchartJsonConverter>()
        }
    };

    private T Create<T>() => ActivatorUtilities.CreateInstance<T>(_serviceProvider);
}