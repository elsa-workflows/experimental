using System.Reflection;

namespace Elsa.Api.Core.Services
{
    public interface IActivityPropertyOptionsProvider
    {
        object? GetOptions(PropertyInfo property);
    }
}