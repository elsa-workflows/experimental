using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Actors;
using Elsa.Runtime.ProtoActor.Messages;
using Elsa.Runtime.Stimuli;
using Proto;
using Proto.Cluster;

namespace Elsa.Runtime.ProtoActor.Services
{
    public class ProtoActorStimulusDispatcher : IStimulusDispatcher
    {
        private readonly ActorSystem _actorSystem;

        public ProtoActorStimulusDispatcher(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }
        
        public async Task DispatchStimulus(IStimulus stimulus, CancellationToken cancellationToken = default)
        {
            var standardStimulus = (StandardStimulus)stimulus;
            
            var message = new HandleStimulus
            {
                ActivityTypeName = standardStimulus.ActivityTypeName,
                Hash = standardStimulus.Hash
            };
            
            var id = nameof(WorkflowServerActor);
            
            var actor = await _actorSystem.Cluster().RequestAsync<WorkflowServerActor>(id, "Execute", message, cancellationToken);
        }
    }
}