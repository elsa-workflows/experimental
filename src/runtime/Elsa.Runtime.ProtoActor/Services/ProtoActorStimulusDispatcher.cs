using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Actors;
using Elsa.Runtime.ProtoActor.Messages;
using Elsa.Runtime.Stimuli;
using Proto.Cluster;

namespace Elsa.Runtime.ProtoActor.Services
{
    public class ProtoActorStimulusDispatcher : IStimulusDispatcher
    {
        private readonly Cluster _cluster;

        public ProtoActorStimulusDispatcher(Cluster cluster)
        {
            _cluster = cluster;
        }
        
        public async Task DispatchStimulus(IStimulus stimulus, CancellationToken cancellationToken = default)
        {
            var standardStimulus = (StandardStimulus)stimulus;
            
            var message = new HandleStimulusRequest
            {
                ActivityTypeName = standardStimulus.ActivityTypeName,
                Hash = standardStimulus.Hash
            };
            
            const string? id = nameof(WorkflowServerActor);
            await _cluster.RequestAsync<Ack>(id, GrainKinds.WorkflowServer, message, cancellationToken);
        }
    }
}