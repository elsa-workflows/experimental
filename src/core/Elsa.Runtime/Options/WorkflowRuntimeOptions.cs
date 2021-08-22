using System;
using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Runtime.Options
{
    public class WorkflowRuntimeOptions
    {
        /// <summary>
        /// A list of workflow factories configured at application startup.
        /// </summary>
        public IDictionary<string, Func<IActivity>> WorkflowFactories { get; set; } = new Dictionary<string, Func<IActivity>>();
    }
}