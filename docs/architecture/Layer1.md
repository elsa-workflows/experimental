# Layer 1: Core protocol

This document describes the core architecture of the envisioned workflow engine.
The library separates core layer 1 engine functionality from higher layers.

## Node

A node represents a single element within a node graph that can be executed and is implemented as a class implementing `INode`.

```csharp
interface INode
{
   string Name { get; set; }
}
```

The node itself does not execute logic. This is instead handled by a corresponding node handler.

### Node Properties

Node properties exist in 3 categories:

1. Input properties.
2. Output properties.
3. Ports.

### Input Properties

An input property represents activity input that can be configured at design time. These properties can be simple primitive types, complex objects and workflow expressions.

### Output Properties

When a node handler executes, it receives the node object itself. The handlers can update the node if it has output properties declared.

### Ports

A port represents a reference to a connected node. The node handler can schedule these nodes. For example, the `If` node has two ports: `True` and `False`.
When the handler evaluates the condition (provided as an input property), it will schedule either the node stored in the `True` property or `False` property.

Example:

```csharp
class IfElse : INode
{
   [Input] IExpression<bool> Condition { get; set; }
   [Port] INode True { get; set; }
   [Port] INode False { get; set; }
}

class IfDriver : NodeDriver<If>
{
   IfDriver(IExpressionEvaluator expressionEvaluator)
   {
      _expressionEvaluator = expressionEvaluator;
   }

   override async ValueTask<INodeExecutionResult> ExecuteAsync(If node, NodeExecutionContext context)
   {
      var result = _expressionEvaluator.EvaluateAsync(node.Condition, context.CancellationToken);
      var nextNode = result ? True : False;
      
      return new ScheduleNodeResult(nextNode);
   }
}
```

## Node Driver

A node driver declares what node type they can execute and has a single `ExecuteAsync` method.

```charp
class NodeExecutionContext
{
   INode Node { get; }
   CancellationToken CancellationToken { get; }
}

interface INodeDriver
{
   ValueTask<INodeExecutionResult> ExecuteAsync(NodeExecutionContext context);
}

abstract class NodeDriver<TNode> : INodeDriver
{
   ValueTask<INodeExecutionResult> INodeDriver.ExecuteAsync(NodeExecutionContext context) => ExecuteAsync((TNode)context.Node, context);
   protected abstract ValueTask<INodeExecutionResult> ExecuteAsync(TNode node, NodeExecutionContext context);
}
```

## Container Nodes

A container node is just like a regular node, but with the following additional properties:

- `Nodes: ICollection<INode>`
- `Variables: IDictionary<string, object>`

## Middleware Pipelines

The engine has different pipelines at different levels.

- Node Execution Pipeline

### Node Execution Pipeline

This pipeline executes each middleware when executing a node.