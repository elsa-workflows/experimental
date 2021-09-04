using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Runtime.Models;
using Elsa.WorkflowBuilderApi.Contracts;

namespace Elsa.WorkflowBuilderApi
{
    public class WorkflowBuilder : IWorkflowBuilder
    {
        public IActivity? Root { get; set; }
        public ICollection<Trigger> Triggers { get; } = new List<Trigger>();
    }
}