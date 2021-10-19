using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Elsa.Runtime.ProtoActor.Messages;
using Proto.Cluster;

namespace Elsa.Runtime.ProtoActor.Services
{
    public class ProtoActorWorkflowDefinitionDispatcher : IWorkflowDefinitionDispatcher
    {
        private readonly Cluster _cluster;

        public ProtoActorWorkflowDefinitionDispatcher(Cluster cluster)
        {
            _cluster = cluster;
        }

        public async Task DispatchAsync(DispatchWorkflowDefinitionRequest request, CancellationToken cancellationToken = default)
        {
            var (definitionId, version) = request;
            var workflowDefinitionIdAndVersion = $"{definitionId}:{version}";

            var message = new ExecuteWorkflowDefinition
            {
                Id = definitionId,
                Version = version
            };

            await _cluster.RequestAsync<Ack>(workflowDefinitionIdAndVersion, GrainKinds.WorkflowDefinition, message, cancellationToken);
        }
    }
}