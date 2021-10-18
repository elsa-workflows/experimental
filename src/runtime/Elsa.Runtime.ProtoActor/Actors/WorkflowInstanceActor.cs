using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Messages;
using Proto;
using Bookmark = Elsa.Models.Bookmark;

namespace Elsa.Runtime.ProtoActor.Actors
{
    public class WorkflowInstanceActor : IActor
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowInvoker _workflowInvoker;

        public WorkflowInstanceActor(IWorkflowInstanceStore workflowInstanceStore, IWorkflowRegistry workflowRegistry, IWorkflowInvoker workflowInvoker)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _workflowInvoker = workflowInvoker;
        }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            ExecuteWorkflowInstance m => OnExecuteWorkflowInstance(context, m),
            _ => Task.CompletedTask
        };

        private async Task OnExecuteWorkflowInstance(IContext context, ExecuteWorkflowInstance message)
        {
            var workflowInstanceId = message.Id;
            var cancellationToken = context.CancellationToken;
            var workflowInstance = await _workflowInstanceStore.GetByIdAsync(workflowInstanceId, cancellationToken);

            if (workflowInstance == null)
                throw new Exception($"No workflow instance found with ID {workflowInstanceId}");

            var workflowDefinitionId = workflowInstance.DefinitionId;
            var workflowDefinition = await _workflowRegistry.GetByIdAsync(workflowDefinitionId, cancellationToken);

            if (workflowDefinition == null)
                throw new Exception($"No workflow definition found with ID {workflowDefinitionId}");

            var workflowState = workflowInstance.WorkflowState;
            var bookmarkMessage = message.Bookmark;

            if (bookmarkMessage == null)
            {
                await _workflowInvoker.InvokeAsync(workflowDefinition, cancellationToken);
            }
            else
            {
                var bookmark =
                    new Bookmark(
                        bookmarkMessage.Id,
                        bookmarkMessage.Hash,
                        bookmarkMessage.Name,
                        bookmarkMessage.ActivityId,
                        bookmarkMessage.ActivityInstanceId,
                        null,
                        bookmarkMessage.CallbackMethodName);

                await _workflowInvoker.InvokeAsync(workflowDefinition, workflowState, bookmark, cancellationToken);
            }
        }
    }
}