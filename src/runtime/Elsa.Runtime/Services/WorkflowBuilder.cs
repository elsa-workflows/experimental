using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Services
{
    public class WorkflowBuilder : IWorkflowBuilder
    {
        public IActivity Root { get; set; } = default!;
        public ICollection<ITrigger> Triggers { get; set; } = new List<ITrigger>();
    }
}