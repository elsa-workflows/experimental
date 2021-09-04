using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.WorkflowBuilderApi.Contracts
{
    public interface IWorkflowBuilder
    {
        IActivity Root { get; set; }
        ICollection<Trigger> Triggers { get; }
    }
}