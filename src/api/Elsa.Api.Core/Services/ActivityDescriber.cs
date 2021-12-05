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

public class ActivityDescriber : IActivityDescriber
{
    private readonly IActivityPropertyOptionsResolver _optionsResolver;
    private readonly IActivityPropertyDefaultValueResolver _defaultValueResolver;
    private readonly IActivityFactory _activityFactory;

    public ActivityDescriber(IActivityPropertyOptionsResolver optionsResolver, IActivityPropertyDefaultValueResolver defaultValueResolver, IActivityFactory activityFactory)
    {
        _optionsResolver = optionsResolver;
        _defaultValueResolver = defaultValueResolver;
        _activityFactory = activityFactory;
    }

    public ValueTask<ActivityDescriptor> DescribeActivityAsync(Type activityType, CancellationToken cancellationToken = default)
    {
        var ns = ActivityTypeNameHelper.GenerateActivityTypeNamespace(activityType);
        var typeName = activityType.Name;
        var fullTypeName = ActivityTypeNameHelper.GenerateActivityTypeName(activityType, ns);
        var displayNameAttr = activityType.GetCustomAttribute<DisplayNameAttribute>();
        var displayName = displayNameAttr?.DisplayName ?? typeName.Humanize(LetterCasing.Title);
        var categoryAttr = activityType.GetCustomAttribute<CategoryAttribute>();
        var category = categoryAttr?.Category ?? GetCategoryFromNamespace(ns) ?? "Miscellaneous";
        var descriptionAttr = activityType.GetCustomAttribute<DescriptionAttribute>();
        var description = descriptionAttr?.Description;

        var outboundPorts =
            from prop in activityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            where typeof(IActivity).IsAssignableFrom(prop.PropertyType) || typeof(IEnumerable<IActivity>).IsAssignableFrom(prop.PropertyType)
            let portAttr = prop.GetCustomAttribute<OutboundAttribute>()
            where portAttr != null
            select new Port
            {
                Name = portAttr.Name ?? prop.Name,
                DisplayName = portAttr.DisplayName ?? portAttr.Name ?? prop.Name
            };

        var properties = activityType.GetProperties();
        var inputProperties = properties.Where(x => typeof(Input).IsAssignableFrom(x.PropertyType)).ToList();
        var outputProperties = properties.Where(x => typeof(Output).IsAssignableFrom(x.PropertyType)).ToList();
        var isTrigger = activityType.IsAssignableTo(typeof(ITrigger));

        var descriptor = new ActivityDescriptor
        {
            Category = category,
            Description = description,
            ActivityType = fullTypeName,
            DisplayName = displayName,
            Traits = isTrigger ? ActivityTraits.Trigger : ActivityTraits.Action,
            OutboundPorts = outboundPorts.ToList(),
            InputProperties = DescribeInputProperties(inputProperties).ToList(),
            OutputProperties = DescribeOutputProperties(outputProperties).ToList(),
            Constructor = context =>
            {
                var activity = _activityFactory.Create(activityType, context);
                activity.ActivityType = fullTypeName;
                return activity;
            }
        };

        return ValueTask.FromResult(descriptor);
    }

    private string? GetCategoryFromNamespace(string? ns)
    {
        if (string.IsNullOrWhiteSpace(ns))
            return null;

        var index = ns.LastIndexOf('.');

        return index < 0 ? ns : ns[index..];
    }

    private IEnumerable<ActivityInputDescriptor> DescribeInputProperties(IEnumerable<PropertyInfo> properties)
    {
        foreach (var propertyInfo in properties)
        {
            var inputAttribute = propertyInfo.GetCustomAttribute<InputAttribute>();
            var descriptionAttribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            var wrappedPropertyType = propertyInfo.PropertyType.GenericTypeArguments[0];

            yield return new ActivityInputDescriptor
            (
                inputAttribute?.Name ?? propertyInfo.Name,
                wrappedPropertyType,
                GetUIHint(wrappedPropertyType, inputAttribute),
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

    private IEnumerable<ActivityOutputDescriptor> DescribeOutputProperties(IEnumerable<PropertyInfo> properties)
    {
        foreach (var propertyInfo in properties)
        {
            var activityPropertyAttribute = propertyInfo.GetCustomAttribute<OutputAttribute>();
            var wrappedPropertyType = propertyInfo.PropertyType.GenericTypeArguments[0];

            yield return new ActivityOutputDescriptor
            (
                (activityPropertyAttribute?.Name ?? propertyInfo.Name).Pascalize(),
                wrappedPropertyType,
                activityPropertyAttribute?.Description
            );
        }
    }

    private string GetUIHint(Type wrappedPropertyType, InputAttribute? inputAttribute)
    {
        if (inputAttribute?.UIHint != null)
            return inputAttribute.UIHint;

        if (wrappedPropertyType == typeof(bool) || wrappedPropertyType == typeof(bool?))
            return ActivityInputUIHints.Checkbox;

        if (wrappedPropertyType == typeof(string))
            return ActivityInputUIHints.SingleLine;

        if (typeof(IEnumerable).IsAssignableFrom(wrappedPropertyType))
            return ActivityInputUIHints.Dropdown;

        if (wrappedPropertyType.IsEnum || wrappedPropertyType.IsNullableType() && wrappedPropertyType.GetTypeOfNullable().IsEnum)
            return ActivityInputUIHints.Dropdown;

        return ActivityInputUIHints.SingleLine;
    }
}