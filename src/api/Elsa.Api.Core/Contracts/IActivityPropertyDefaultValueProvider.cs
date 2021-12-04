using System.Reflection;

namespace Elsa.Api.Core.Contracts;

public interface IActivityPropertyDefaultValueProvider
{
    object GetDefaultValue(PropertyInfo property);
}