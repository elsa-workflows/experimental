using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
using Elsa.Runtime.ProtoActor.Messages;
using Proto.Cluster;

namespace Elsa.Runtime.ProtoActor.Services
{
    public class ProtoActorWorkflowInstanceDispatcher : IWorkflowInstanceDispatcher
    {
        private readonly Cluster _cluster;

        public ProtoActorWorkflowInstanceDispatcher(Cluster cluster)
        {
            _cluster = cluster;
        }

        public async Task DispatchAsync(DispatchWorkflowInstanceRequest request, CancellationToken cancellationToken = default)
        {
            var (instanceId, bookmark) = request;
            var grainId = instanceId;
            var bookmarkMessage = MapBookmark(bookmark);
            
            var message = new ExecuteWorkflowInstance
            {
                Id = instanceId,
                Bookmark = bookmarkMessage
            };

            await _cluster.RequestAsync<Ack>(grainId, GrainKinds.WorkflowInstance, message, cancellationToken);
        }

        private Bookmark? MapBookmark(Elsa.Models.Bookmark? bookmark)
        {
            if (bookmark == null)
                return null;

            return new Bookmark
            {
                Id = bookmark.Id,
                Name = bookmark.Name,
                Hash = bookmark.Hash,
                ActivityId = bookmark.ActivityId,
                ActivityInstanceId = bookmark.ActivityInstanceId,
                CallbackMethodName = bookmark.CallbackMethodName,
                Data = { }
            };
        }
    }
}