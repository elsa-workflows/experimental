# Management

This document describes the architecture of the envisioned workflow engine.
The library separates core low-level engine functionality from higher-level parts. 

At the management level, the library deals with workflows and workflow instances as well as repository services to store and load workflows.

## Workflow

A workflow holds both the node graph as well as metadata about the workflow, such as the ID, a name, version, ACL, ID, etc.

```csharp
class Workflow
{
   string Id { get; set; }
   string Name { get; set; }
   int Version { get; set; }
   INode Root { get; set; }
}
```

## Workflow Instance

A workflow instance stores information about node output and is what enables workflows to be resumed.

```csharp
class WorkflowInstance
{
   string Id { get; set; }
   string WorkflowId { get; set; }
   int WorkflowVersion { get; set; }
   IDictionary<string, IDictionary<string, object>> NodeOutput { get; set; }
}
```