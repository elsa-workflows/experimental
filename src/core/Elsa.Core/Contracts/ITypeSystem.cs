using Elsa.Models;

namespace Elsa.Contracts
{
    public interface ITypeSystem
    {
        void Register(TypeDescriptor descriptor);
        TypeDescriptor? ResolveTypeName(string typeName);
    }
}