using System.Reflection;

namespace Elsa.Api.Core.Contracts;

public interface IPropertyOptionsResolver
{
    object? GetOptions(PropertyInfo propertyInfo);
}