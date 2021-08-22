# Layer 1: Core protocol

This document describes the core architecture of the envisioned workflow engine.
The library separates core layer 1 engine functionality from higher layers.

## Activity

A activity represents a single element within a activity graph that can be executed and is implemented as a class implementing `IActivity`.

```csharp
interface IActivity
{
   string Name { get; set; }
}
```

The activity itself does not execute logic. This is instead handled by a corresponding activity handler.

### Activity Properties

Activity properties exist in 3 categories:

1. Input properties.
2. Output properties.
3. Ports.

### Input Properties

An input property represents activity input that can be configured at design time. These properties can be simple primitive types, complex objects and workflow expressions.

### Output Properties

When a activity handler executes, it receives the activity object itself. The handlers can update the activity if it has output properties declared.

### Ports

A port represents a reference to a connected activity. The activity handler can schedule these activities. For example, the `If` activity has two ports: `True` and `False`.
When the handler evaluates the condition (provided as an input property), it will schedule either the activity stored in the `True` property or `False` property.

Example:

```csharp
class IfElse : IActivity
{
   [Input] IExpression<bool> Condition { get; set; }
   [Port] IActivity True { get; set; }
   [Port] IActivity False { get; set; }
}

class IfDriver : ActivityDriver<If>
{
   IfDriver(IExpressionEvaluator expressionEvaluator)
   {
      _expressionEvaluator = expressionEvaluator;
   }

   override async ValueTask<INodeExecutionResult> ExecuteAsync(If activity, NodeExecutionContext context)
   {
      var result = _expressionEvaluator.EvaluateAsync(activity.Condition, context.CancellationToken);
      var nextNode = result ? True : False;
      
      return new ScheduleNodeResult(nextNode);
   }
}
```

## Activity Driver

An activity driver declares what activity type they can execute and has a single `ExecuteAsync` method.

```charp
class NodeExecutionContext
{
   IActivity Activity { get; }
   CancellationToken CancellationToken { get; }
}

interface IActivityDriver
{
   ValueTask ExecuteAsync(NodeExecutionContext context);
}

abstract class ActivityDriver<TActivity> : IActivityDriver
{
   ValueTask IActivityDriver.ExecuteAsync(NodeExecutionContext context) => ExecuteAsync((TActivity)context.Activity, context);
   protected abstract ValueTask ExecuteAsync(TActivity activity, NodeExecutionContext context);
}
```

## Container Activities

A container activity is just like a regular activity, but with the following additional properties:

- `Activities: ICollection<IActivity>`
- `Variables: IDictionary<string, object>`

## Middleware Pipelines

The engine has different pipelines at different levels.

- Activity Execution Pipeline

### Activity Execution Pipeline

This pipeline executes each middleware when executing a activity.