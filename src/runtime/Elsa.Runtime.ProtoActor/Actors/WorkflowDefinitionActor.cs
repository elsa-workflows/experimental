using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Messages;
using Elsa.State;
using Proto;
using Proto.DependencyInjection;

namespace Elsa.Runtime.ProtoActor.Actors
{
    public class WorkflowDefinitionActor : IActor
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public WorkflowDefinitionActor(IWorkflowRegistry workflowRegistry, IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowRegistry = workflowRegistry;
            _workflowInstanceStore = workflowInstanceStore;
        }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            ExecuteWorkflowDefinition i => OnInstantiateAndScheduleWorkflowDefinition(context, i),
            _ => Task.CompletedTask
        };

        private async Task OnInstantiateAndScheduleWorkflowDefinition(IContext context, ExecuteWorkflowDefinition message)
        {
            var workflowDefinitionId = message.Id;
            var cancellationToken = context.CancellationToken;
            var workflowDefinition = (await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken))!;
            var workflowInstanceId = Guid.NewGuid().ToString();

            var workflowInstance = new WorkflowInstance
            {
                Id = workflowInstanceId,
                Version = workflowDefinition.Version,
                DefinitionId = workflowDefinitionId,
                WorkflowState = new WorkflowState
                {
                    Id = workflowInstanceId
                }
            };

            await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);
            var props = context.System.DI().PropsFor<WorkflowInstanceActor>();
            var pid = context.SpawnNamed(props, workflowInstanceId);

            context.Send(pid, new ExecuteWorkflowInstance
            {
                Id = workflowInstanceId
            });
        }
    }
}