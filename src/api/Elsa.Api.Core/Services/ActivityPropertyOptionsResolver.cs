using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Extensions;
using Elsa.Api.Core.Models;
using Elsa.Attributes;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Api.Core.Services;

public class ActivityPropertyOptionsResolver : IActivityPropertyOptionsResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ActivityPropertyOptionsResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetOptions(PropertyInfo activityPropertyInfo)
    {
        var inputAttribute = activityPropertyInfo.GetCustomAttribute<InputAttribute>();

        if (inputAttribute == null)
            return null;

        if (inputAttribute.OptionsProvider == null)
            return inputAttribute.Options ?? (TryGetEnumOptions(activityPropertyInfo, out var items) ? items : null);

        var providerType = inputAttribute.OptionsProvider;

        using var scope = _serviceProvider.CreateScope();
        var provider = (IActivityPropertyOptionsProvider) ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, providerType);
        return provider.GetOptions(activityPropertyInfo);
    }

    private bool TryGetEnumOptions(PropertyInfo activityPropertyInfo, out IList<SelectListItem>? items)
    {
        var isNullable = activityPropertyInfo.PropertyType.IsNullableType();
        var propertyType = isNullable ? activityPropertyInfo.PropertyType.GetTypeOfNullable() : activityPropertyInfo.PropertyType;

        items = null;

        if (!propertyType.IsEnum)
            return false;
            
        items = propertyType.GetEnumNames().Select(x => new SelectListItem(x.Humanize(LetterCasing.Title), x)).ToList();

        if (isNullable)
            items.Insert(0, new SelectListItem("-", ""));

        return true;
    }
}