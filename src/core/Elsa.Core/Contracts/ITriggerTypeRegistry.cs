using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface ITriggerTypeRegistry
    {
        void Register(TriggerType triggerType);
        IEnumerable<TriggerType> List();
        TriggerType Get(string typeName);
    }
}