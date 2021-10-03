using System;
using System.Collections.Generic;
using Elsa.Activities.Containers;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Builders
{
    public class WorkflowDefinitionBuilder : IWorkflowDefinitionBuilder
    {
        private string? _id;
        private int _version = 1;
        private IActivity? _root;
        private readonly ICollection<ITrigger> _triggers = new List<ITrigger>();

        public IWorkflowDefinitionBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public IWorkflowDefinitionBuilder WithVersion(int version)
        {
            _version = version;
            return this;
        }

        public IWorkflowDefinitionBuilder WithRoot(IActivity root)
        {
            _root = root;
            return this;
        }

        public IWorkflowDefinitionBuilder AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
            return this;
        }

        public WorkflowDefinition BuildWorkflow()
        {
            var id = _id ?? Guid.NewGuid().ToString("N");
            var root = _root ?? new Sequence();

            return new WorkflowDefinition
            {
                Id = Guid.NewGuid().ToString("N"),
                Version = _version,
                CreatedAt = DateTime.UtcNow,
                DefinitionId = id,
                Workflow = new Workflow(root, _triggers)
            };
        }
    }
}