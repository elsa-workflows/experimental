using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.ProtoActor.Actors;
using Microsoft.Extensions.Hosting;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;

namespace Elsa.Runtime.ProtoActor.HostedServices
{
    public class WorkflowServerHost : BackgroundService
    {
        private readonly ActorSystem _actorSystem;

        public WorkflowServerHost(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
             var props = _actorSystem.DI().PropsFor<WorkflowServerActor>();
             _actorSystem.Root.SpawnNamed(props, nameof(WorkflowServerActor));

            // await _actorSystem.Cluster().StartMemberAsync();
            // await _actorSystem.Cluster().StartClientAsync();
        }
    }
}