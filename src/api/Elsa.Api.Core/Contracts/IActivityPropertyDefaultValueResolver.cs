using System.Reflection;

namespace Elsa.Api.Core.Contracts;

public interface IActivityPropertyDefaultValueResolver
{
    object? GetDefaultValue(PropertyInfo activityPropertyInfo);
}