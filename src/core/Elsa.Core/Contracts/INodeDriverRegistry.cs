using System;

namespace Elsa.Contracts
{
    public interface INodeDriverRegistry
    {
        void Register(Type nodeType, Type driverType);
        
        public INodeDriver? GetDriver(INode node)
        {
            throw new NotImplementedException();
        }   INodeDriver? GetDriver(Type nodeType);
    }
}