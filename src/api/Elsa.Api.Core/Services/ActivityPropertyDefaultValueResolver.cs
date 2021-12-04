using System;
using System.Reflection;
using Elsa.Api.Core.Contracts;
using Elsa.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Core.Services;

public class ActivityPropertyDefaultValueResolver : IActivityPropertyDefaultValueResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ActivityPropertyDefaultValueResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetDefaultValue(PropertyInfo activityPropertyInfo)
    {
        var inputAttribute = activityPropertyInfo.GetCustomAttribute<InputAttribute>();

        if (inputAttribute == null)
            return null;

        if (inputAttribute.DefaultValueProvider == null)
            return inputAttribute.DefaultValue;

        var providerType = inputAttribute.DefaultValueProvider;

        using var scope = _serviceProvider.CreateScope();
        var provider = (IActivityPropertyDefaultValueProvider) ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, providerType);
        return provider.GetDefaultValue(activityPropertyInfo);
    }
}