using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Actors;
using Elsa.Runtime.ProtoActor.Messages;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;

namespace Elsa.Runtime.ProtoActor.Services
{
    public class ProtoActorWorkflowDefinitionDispatcher : IWorkflowDefinitionDispatcher
    {
        private readonly ActorSystem _actorSystem;

        public ProtoActorWorkflowDefinitionDispatcher(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        public Task DispatchAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default)
        {
            var workflowDefinitionIdAndVersion = $"{workflowDefinition.Id}:{workflowDefinition.Version}";

            var message = new ExecuteWorkflowDefinition
            {
                Id = workflowDefinition.DefinitionId,
                Version = workflowDefinition.Version
            };

            //var actor = await _actorSystem.Cluster().RequestAsync<WorkflowDefinitionActor>(workflowDefinitionIdAndVersion, "Execute", message, cancellationToken);

            var pid = _actorSystem.ProcessRegistry.SearchByName(workflowDefinitionIdAndVersion).FirstOrDefault();

            if (pid == null)
            {
                var props = _actorSystem.DI().PropsFor<WorkflowDefinitionActor>();
                pid = _actorSystem.Root.SpawnNamed(props, workflowDefinitionIdAndVersion);
            }
            
            _actorSystem.Root.Send(pid, message);
            return Task.CompletedTask;
        }
    }
}