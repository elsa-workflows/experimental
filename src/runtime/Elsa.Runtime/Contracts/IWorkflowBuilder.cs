using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowBuilder
    {
        IActivity Root { get; set; }
        ICollection<TriggerSource> Triggers { get; }
    }
}