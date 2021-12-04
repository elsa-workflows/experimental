using System;
using System.Text.Json;
using Elsa.Api.Converters;
using Elsa.Api.Core.Contracts;

namespace Elsa.Api.Serializers;

public class WorkflowSerializerOptionsProvider
{
    private readonly IWellKnownTypeRegistry _wellKnownTypeRegistry;
    private readonly IActivityRegistry _activityRegistry;
    private readonly IServiceProvider _serviceProvider;

    public WorkflowSerializerOptionsProvider(IWellKnownTypeRegistry wellKnownTypeRegistry, IActivityRegistry activityRegistry, IServiceProvider serviceProvider)
    {
        _wellKnownTypeRegistry = wellKnownTypeRegistry;
        _activityRegistry = activityRegistry;
        _serviceProvider = serviceProvider;
    }
    
    public JsonSerializerOptions CreateSerializerOptions() => new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new TypeJsonConverter(_wellKnownTypeRegistry),
            new ActivityJsonConverterFactory(_activityRegistry, _serviceProvider),
        }
    };
}