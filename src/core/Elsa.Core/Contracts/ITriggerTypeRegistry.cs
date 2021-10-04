using Elsa.Models;

namespace Elsa.Contracts
{
    public interface ITriggerTypeRegistry : IRegistry<TriggerType>
    {
        void Register(TriggerType value);
    }
}