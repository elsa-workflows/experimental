using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface IRegistry<T>
    {
        void Register(string key, T value);
        IEnumerable<T> List();
        T Get(string name);
    }
}