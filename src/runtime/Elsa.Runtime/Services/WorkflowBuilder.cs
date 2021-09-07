using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Services
{
    public class WorkflowBuilder : IWorkflowBuilder
    {
        public IActivity Root { get; set; } = default!;
        public ICollection<TriggerSource> Triggers { get; } = new List<TriggerSource>();
    }
}