using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Runtime.Contracts
{
    public interface IWorkflowBuilder
    {
        IActivity Root { get; set; }
        ICollection<ITrigger> Triggers { get; set; }
    }
}