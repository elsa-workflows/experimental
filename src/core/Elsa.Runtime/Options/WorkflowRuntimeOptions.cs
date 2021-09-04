using System;
using System.Collections.Generic;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Options
{
    public class WorkflowRuntimeOptions
    {
        /// <summary>
        /// A list of workflow factories configured at application startup.
        /// </summary>
        public IDictionary<string, Func<WorkflowDefinition>> WorkflowFactories { get; set; } = new Dictionary<string, Func<WorkflowDefinition>>();
    }
}