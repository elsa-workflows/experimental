using Elsa.Activities.Workflows;
using Elsa.Contracts;
using Elsa.Management.Contracts;
using Elsa.Management.Notifications;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Models;
using Elsa.Persistence.Requests;

namespace Elsa.Management.Services
{
    public class WorkflowPublisher : IWorkflowPublisher
    {
        private readonly IMediator _mediator;
        private readonly IIdentityGenerator _identityGenerator;
        private readonly ISystemClock _systemClock;

        public WorkflowPublisher(IMediator mediator, IIdentityGenerator identityGenerator, ISystemClock systemClock)
        {
            _mediator = mediator;
            _identityGenerator = identityGenerator;
            _systemClock = systemClock;
        }

        public WorkflowDefinition New()
        {
            var id = _identityGenerator.GenerateId();
            const int version = 1;

            var definition = new WorkflowDefinition
            {
                Id = $"{id}-{version}",
                DefinitionId = id,
                Version = version,
                IsLatest = true,
                IsPublished = false,
                Root = new Sequence(),
                Triggers = new List<ITrigger>(),
                CreatedAt = _systemClock.UtcNow
            };

            return definition;
        }

        public async Task<WorkflowDefinition?> PublishAsync(string definitionId, CancellationToken cancellationToken = default)
        {
            var definition = await _mediator.RequestAsync(new FindWorkflowDefinition(definitionId, VersionOptions.Latest), cancellationToken);

            if (definition == null)
                return null;

            return await PublishAsync(definition, cancellationToken);
        }

        public async Task<WorkflowDefinition> PublishAsync(WorkflowDefinition definition, CancellationToken cancellationToken = default)
        {
            var definitionId = definition.DefinitionId;

            // Reset current latest and published definitions.
            var publishedAndOrLatestDefinitions = await _mediator.RequestAsync(new FindLatestAndPublishedWorkflowDefinitions(definitionId), cancellationToken);

            foreach (var publishedAndOrLatestDefinition in publishedAndOrLatestDefinitions)
            {
                publishedAndOrLatestDefinition.IsPublished = false;
                publishedAndOrLatestDefinition.IsLatest = false;
                await _mediator.ExecuteAsync(new SaveWorkflowDefinition(publishedAndOrLatestDefinition), cancellationToken);
            }

            if (definition.IsPublished)
                definition.Version++;
            else
                definition.IsPublished = true;

            definition.IsLatest = true;
            definition = Initialize(definition);

            await _mediator.PublishAsync(new WorkflowDefinitionPublishing(definition), cancellationToken);
            await _mediator.ExecuteAsync(new SaveWorkflowDefinition(definition), cancellationToken);
            await _mediator.PublishAsync(new WorkflowDefinitionPublished(definition), cancellationToken);
            return definition;
        }

        public async Task<WorkflowDefinition?> RetractAsync(string definitionId, CancellationToken cancellationToken = default)
        {
            var definition = await _mediator.RequestAsync(new FindWorkflowDefinition(definitionId, VersionOptions.Published), cancellationToken);

            if (definition == null)
                return null;

            return await RetractAsync(definition, cancellationToken);
        }

        public async Task<WorkflowDefinition> RetractAsync(WorkflowDefinition definitionId, CancellationToken cancellationToken = default)
        {
            if (!definitionId.IsPublished)
                throw new InvalidOperationException("Cannot unpublish an unpublished workflow definition.");

            definitionId.IsPublished = false;
            definitionId = Initialize(definitionId);

            await _mediator.PublishAsync(new WorkflowDefinitionRetracting(definitionId), cancellationToken);
            await _mediator.ExecuteAsync(new SaveWorkflowDefinition(definitionId), cancellationToken);
            await _mediator.PublishAsync(new WorkflowDefinitionRetracted(definitionId), cancellationToken);
            return definitionId;
        }

        public async Task<WorkflowDefinition?> GetDraftAsync(string definitionId, CancellationToken cancellationToken = default)
        {
            var definition = await _mediator.RequestAsync(new FindWorkflowDefinition(definitionId, VersionOptions.Latest), cancellationToken);

            if (definition == null)
                return null;

            if (!definition.IsPublished)
                return definition;

            var draft = definition.ShallowClone();
            draft.Id = _identityGenerator.GenerateId();
            draft.IsPublished = false;
            draft.IsLatest = true;
            draft.Version++;

            return draft;
        }

        public async Task<WorkflowDefinition> SaveDraftAsync(WorkflowDefinition definition, CancellationToken cancellationToken = default)
        {
            var draft = definition;
            var latestVersion = await _mediator.RequestAsync(new FindWorkflowDefinition(definition.DefinitionId, VersionOptions.Latest), cancellationToken);

            if (latestVersion is { IsPublished: true, IsLatest: true })
            {
                latestVersion.IsLatest = false;
                await _mediator.ExecuteAsync(new SaveWorkflowDefinition(latestVersion), cancellationToken);
            }

            draft.IsLatest = true;
            draft.IsPublished = false;
            draft = Initialize(draft);

            await _mediator.ExecuteAsync(new SaveWorkflowDefinition(draft), cancellationToken);
            return draft;
        }

        public async Task DeleteAsync(string definitionId, CancellationToken cancellationToken = default)
        {
            await _mediator.ExecuteAsync(new DeleteWorkflowInstances(definitionId), cancellationToken);
            await _mediator.ExecuteAsync(new DeleteWorkflowDefinition(definitionId), cancellationToken);
        }

        public Task DeleteAsync(WorkflowDefinition definitionId, CancellationToken cancellationToken = default) => DeleteAsync(definitionId.Id, cancellationToken);

        private WorkflowDefinition Initialize(WorkflowDefinition workflowDefinition)
        {
            if (workflowDefinition.Id == null!)
                workflowDefinition.Id = _identityGenerator.GenerateId();

            if (workflowDefinition.Version == 0)
                workflowDefinition.Version = 1;

            if (workflowDefinition.DefinitionId == null!)
                workflowDefinition.DefinitionId = _identityGenerator.GenerateId();

            return workflowDefinition;
        }
    }
}