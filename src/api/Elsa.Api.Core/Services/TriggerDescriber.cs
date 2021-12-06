using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Extensions;
using Elsa.Api.Core.Models;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Helpers;
using Elsa.Models;
using Humanizer;

namespace Elsa.Api.Core.Services;

public class TriggerDescriber : ITriggerDescriber
{
    private readonly IPropertyOptionsResolver _optionsResolver;
    private readonly IPropertyDefaultValueResolver _defaultValueResolver;
    private readonly ITriggerFactory _activityFactory;

    public TriggerDescriber(IPropertyOptionsResolver optionsResolver, IPropertyDefaultValueResolver defaultValueResolver, ITriggerFactory activityFactory)
    {
        _optionsResolver = optionsResolver;
        _defaultValueResolver = defaultValueResolver;
        _activityFactory = activityFactory;
    }

    public ValueTask<TriggerDescriptor> DescribeTriggerAsync(Type triggerType, CancellationToken cancellationToken = default)
    {
        var ns = TypeNameHelper.GenerateNamespace(triggerType);
        var typeName = triggerType.Name;
        var fullTypeName = TypeNameHelper.GenerateTypeName(triggerType, ns);
        var displayNameAttr = triggerType.GetCustomAttribute<DisplayNameAttribute>();
        var displayName = displayNameAttr?.DisplayName ?? typeName.Humanize(LetterCasing.Title);
        var categoryAttr = triggerType.GetCustomAttribute<CategoryAttribute>();
        var category = categoryAttr?.Category ?? TypeNameHelper.GetCategoryFromNamespace(ns) ?? "Miscellaneous";
        var descriptionAttr = triggerType.GetCustomAttribute<DescriptionAttribute>();
        var description = descriptionAttr?.Description;
        var properties = triggerType.GetProperties();
        var inputProperties = properties.Where(x => typeof(Input).IsAssignableFrom(x.PropertyType)).ToList();

        var descriptor = new TriggerDescriptor
        {
            Category = category,
            Description = description,
            TriggerType = fullTypeName,
            DisplayName = displayName,
            InputProperties = DescribeInputProperties(inputProperties).ToList(),
            Constructor = context =>
            {
                var activity = _activityFactory.Create(triggerType, context);
                activity.TriggerType = fullTypeName;
                return activity;
            }
        };

        return ValueTask.FromResult(descriptor);
    }

    private IEnumerable<InputDescriptor> DescribeInputProperties(IEnumerable<PropertyInfo> properties)
    {
        foreach (var propertyInfo in properties)
        {
            var inputAttribute = propertyInfo.GetCustomAttribute<InputAttribute>();
            var descriptionAttribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            var wrappedPropertyType = propertyInfo.PropertyType.GenericTypeArguments[0];

            yield return new InputDescriptor
            (
                inputAttribute?.Name ?? propertyInfo.Name,
                wrappedPropertyType,
                InputUIHints.GetUIHint(wrappedPropertyType, inputAttribute),
                inputAttribute?.DisplayName ?? propertyInfo.Name.Humanize(LetterCasing.Title),
                descriptionAttribute?.Description,
                _optionsResolver.GetOptions(propertyInfo),
                inputAttribute?.Category,
                inputAttribute?.Order ?? 0,
                _defaultValueResolver.GetDefaultValue(propertyInfo),
                inputAttribute?.DefaultSyntax,
                inputAttribute?.SupportedSyntaxes,
                inputAttribute?.IsReadOnly ?? false,
                inputAttribute?.IsBrowsable ?? true
            );
        }
    }
}