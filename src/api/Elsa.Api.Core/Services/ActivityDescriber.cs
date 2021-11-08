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
using Humanizer;

namespace Elsa.Api.Core.Services
{
    public class ActivityDescriber : IActivityDescriber
    {
        private readonly IActivityPropertyOptionsResolver _optionsResolver;
        private readonly IActivityPropertyDefaultValueResolver _defaultValueResolver;

        public ActivityDescriber(IActivityPropertyOptionsResolver optionsResolver, IActivityPropertyDefaultValueResolver defaultValueResolver)
        {
            _optionsResolver = optionsResolver;
            _defaultValueResolver = defaultValueResolver;
        }

        public ValueTask<ActivityDescriptor> DescribeActivityAsync(Type activityType, CancellationToken cancellationToken = default)
        {
            var typeName = activityType.Name;
            var displayNameAttr = activityType.GetCustomAttribute<DisplayNameAttribute>();
            var displayName = displayNameAttr?.DisplayName ?? typeName.Humanize(LetterCasing.Title);
            var categoryAttr = activityType.GetCustomAttribute<CategoryAttribute>();
            var category = categoryAttr?.Category ?? "Miscellaneous";
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

            var descriptor = new ActivityDescriptor
            {
                Category = category,
                Description = description,
                ActivityType = typeName,
                DisplayName = displayName,
                OutboundPorts = outboundPorts.ToList(),
                InputProperties = DescribeInputProperties(properties).ToList(),
                OutputProperties = DescribeOutputProperties(properties).ToList()
            };

            return ValueTask.FromResult(descriptor);
        }

        private IEnumerable<ActivityInputDescriptor> DescribeInputProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                var inputAttribute = propertyInfo.GetCustomAttribute<InputAttribute>();

                if (inputAttribute == null)
                    continue;

                yield return new ActivityInputDescriptor
                (
                    inputAttribute.Name ?? propertyInfo.Name,
                    propertyInfo.PropertyType,
                    GetUIHint(propertyInfo),
                    inputAttribute.DisplayName ?? propertyInfo.Name.Humanize(LetterCasing.Title),
                    inputAttribute.Description,
                    _optionsResolver.GetOptions(propertyInfo),
                    inputAttribute.Category,
                    inputAttribute.Order,
                    _defaultValueResolver.GetDefaultValue(propertyInfo),
                    inputAttribute.DefaultSyntax,
                    inputAttribute.SupportedSyntaxes,
                    inputAttribute.IsReadOnly,
                    inputAttribute.IsBrowsable
                );
            }
        }

        private IEnumerable<ActivityOutputDescriptor> DescribeOutputProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                var activityPropertyAttribute = propertyInfo.GetCustomAttribute<OutputAttribute>();

                if (activityPropertyAttribute == null)
                    continue;

                yield return new ActivityOutputDescriptor
                (
                    (activityPropertyAttribute.Name ?? propertyInfo.Name).Pascalize(),
                    propertyInfo.PropertyType,
                    activityPropertyAttribute.Description
                );
            }
        }

        private string GetUIHint(PropertyInfo propertyInfo)
        {
            var activityPropertyAttribute = propertyInfo.GetCustomAttribute<InputAttribute>()!;

            if (activityPropertyAttribute.UIHint != null)
                return activityPropertyAttribute.UIHint;

            var type = propertyInfo.PropertyType;

            if (type == typeof(bool) || type == typeof(bool?))
                return ActivityInputUIHints.Checkbox;

            if (type == typeof(string))
                return ActivityInputUIHints.SingleLine;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return ActivityInputUIHints.Dropdown;

            if (type.IsEnum || type.IsNullableType() && type.GetTypeOfNullable().IsEnum)
                return ActivityInputUIHints.Dropdown;

            return ActivityInputUIHints.SingleLine;
        }
    }
}