using System;

namespace Elsa.Contracts
{
    public interface INodeDriverRegistry
    {
        void Register(Type nodeType, Type driverType);
        INodeDriver? GetDriver(INode node);
        INodeDriver? GetDriver(Type nodeType);
    }
}