using System.Reflection;

namespace Elsa.Api.Core.Contracts;

public interface IPropertyDefaultValueResolver
{
    object? GetDefaultValue(PropertyInfo activityPropertyInfo);
}