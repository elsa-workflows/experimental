using System;
using System.Collections.Generic;
using Elsa.Activities.Containers;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Builders
{
    public class WorkflowDefinitionBuilder : IWorkflowDefinitionBuilder
    {
        public string? Id { get; private set; }
        public int Version { get; private set; } = 1;
        public IActivity? Root { get; private set; }
        public ICollection<ITrigger> Triggers { get; } = new List<ITrigger>();

        public IWorkflowDefinitionBuilder WithId(string id)
        {
            Id = id;
            return this;
        }

        public IWorkflowDefinitionBuilder WithVersion(int version)
        {
            Version = version;
            return this;
        }

        public IWorkflowDefinitionBuilder WithRoot(IActivity root)
        {
            Root = root;
            return this;
        }

        public IWorkflowDefinitionBuilder AddTrigger(ITrigger trigger)
        {
            Triggers.Add(trigger);
            return this;
        }

        public WorkflowDefinition BuildWorkflow()
        {
            var id = Id ?? Guid.NewGuid().ToString("N");
            var root = Root ?? new Sequence();

            return new WorkflowDefinition
            {
                Id = id,
                Version = Version,
                CreatedAt = DateTime.UtcNow,
                DefinitionId = id,
                Workflow = new Workflow(root, Triggers)
            };
        }
    }
}