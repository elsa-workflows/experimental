using System.Reflection;

namespace Elsa.Api.Core.Contracts;

public interface IActivityPropertyOptionsResolver
{
    object? GetOptions(PropertyInfo activityPropertyInfo);
}