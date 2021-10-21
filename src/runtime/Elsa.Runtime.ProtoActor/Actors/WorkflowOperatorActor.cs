using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Messages;
using Elsa.State;
using Proto;
using Bookmark = Elsa.Runtime.ProtoActor.Messages.Bookmark;

namespace Elsa.Runtime.ProtoActor.Actors
{
    public class WorkflowOperatorActor : IActor
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowEngine _workflowEngine;

        public WorkflowOperatorActor(IWorkflowInstanceStore workflowInstanceStore, IWorkflowRegistry workflowRegistry, IWorkflowEngine workflowEngine)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _workflowEngine = workflowEngine;
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

            var executionResult = await ExecuteAsync(workflowDefinition, workflowState, bookmarkMessage, cancellationToken);
            var response = MapResult(executionResult);
            
            context.Respond(response);
        }

        private ExecuteWorkflowResponse MapResult(ExecuteWorkflowResult result)
        {
            var bookmarks = result.Bookmarks.Select(x =>
            {
                var data = x.Data?.ToDictionary(y => y.Key, y => new Json
                {
                    Text = JsonSerializer.Serialize(y.Value)
                }) ?? new Dictionary<string, Json>();

                var bookmark = new Bookmark
                {
                    Id = x.Id
                };

                bookmark.Data.Add(data);
                return bookmark;
            });
            
            var response = new ExecuteWorkflowResponse
            {
                WorkflowState = new Json
                {
                    Text = JsonSerializer.Serialize(result.WorkflowState)
                }
            };

            response.Bookmarks.Add(bookmarks);

            return response;
        }

        private async Task<ExecuteWorkflowResult> ExecuteAsync(WorkflowDefinition workflowDefinition, WorkflowState workflowState, Bookmark? bookmarkMessage, CancellationToken cancellationToken)
        {
            if (bookmarkMessage == null)
                return await _workflowEngine.ExecuteAsync(workflowDefinition, cancellationToken);

            var bookmark =
                new Elsa.Models.Bookmark(
                    bookmarkMessage.Id,
                    bookmarkMessage.Name,
                    bookmarkMessage.Hash,
                    bookmarkMessage.ActivityId,
                    bookmarkMessage.ActivityInstanceId,
                    null,
                    bookmarkMessage.CallbackMethodName);
            
            return await _workflowEngine.ExecuteAsync(workflowDefinition, workflowState, bookmark, cancellationToken);
        }
    }
}