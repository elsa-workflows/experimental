using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Messages;
using Elsa.State;
using Proto;
using Proto.Cluster;
using Unit = Elsa.Runtime.ProtoActor.Messages.Unit;

namespace Elsa.Runtime.ProtoActor.Actors;

/// <summary>
/// Instantiates and dispatches a workflow instance for execution. The workflow instance will be executed by <see cref="WorkflowOperatorActor"/>.
/// </summary>
public class WorkflowDefinitionActor : IActor
{
    private readonly IWorkflowRegistry _workflowRegistry;
    private readonly IMediator _mediator;

    public WorkflowDefinitionActor(IWorkflowRegistry workflowRegistry, IMediator mediator)
    {
        _workflowRegistry = workflowRegistry;
        _mediator = mediator;
    }

    public Task ReceiveAsync(IContext context) => context.Message switch
    {
        ExecuteWorkflowDefinition i => OnExecuteWorkflowDefinitionAsync(context, i),
        DispatchWorkflowDefinition i => OnDispatchWorkflowDefinitionAsync(context, i),
        _ => Task.CompletedTask
    };

    private async Task OnExecuteWorkflowDefinitionAsync(IContext context, ExecuteWorkflowDefinition message)
    {
        var workflowDefinitionId = message.Id;
        var cancellationToken = context.CancellationToken;
        var workflowInstance = await CreateWorkflowInstanceAsync(workflowDefinitionId, cancellationToken);
            
        var executeWorkflowInstanceMessage = new ExecuteWorkflowInstance
        {
            Id = workflowInstance.Id
        };
        
        var response = await context.ClusterRequestAsync<ExecuteWorkflowResponse>(workflowInstance.Id, GrainKinds.WorkflowInstance, executeWorkflowInstanceMessage, cancellationToken);
        context.Respond(response);
    }
        
    private async Task OnDispatchWorkflowDefinitionAsync(IContext context, DispatchWorkflowDefinition message)
    {
        var workflowDefinitionId = message.Id;
        var cancellationToken = context.CancellationToken;
        var workflowInstance = await CreateWorkflowInstanceAsync(workflowDefinitionId, cancellationToken);
            
        var dispatchWorkflowInstanceMessage = new DispatchWorkflowInstance
        {
            Id = workflowInstance.Id
        };
        
        await context.ClusterRequestAsync<Unit>(workflowInstance.Id, GrainKinds.WorkflowInstance, dispatchWorkflowInstanceMessage, cancellationToken);
        context.Respond(new Unit());
    }

    private async Task<WorkflowInstance> CreateWorkflowInstanceAsync(string workflowDefinitionId, CancellationToken cancellationToken)
    {
        var workflow = (await _workflowRegistry.FindByIdAsync(workflowDefinitionId, VersionOptions.Published, cancellationToken))!;
        var workflowInstanceId = Guid.NewGuid().ToString();
        
        var workflowInstance = new WorkflowInstance
        {
            Id = workflowInstanceId,
            Version = workflow.Metadata.Identity.Version,
            DefinitionId = workflowDefinitionId,
            WorkflowState = new WorkflowState
            {
                Id = workflowInstanceId
            }
        };
        
        await _mediator.ExecuteAsync(new SaveWorkflowInstance(workflowInstance), cancellationToken);
        return workflowInstance;
    }
}