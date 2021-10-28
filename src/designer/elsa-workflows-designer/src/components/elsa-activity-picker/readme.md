# elsa-activity-picker



<!-- Auto Generated Below -->


## Properties

| Property | Attribute | Description | Type    | Default     |
| -------- | --------- | ----------- | ------- | ----------- |
| `graph`  | --        |             | `Graph` | `undefined` |


## Events

| Event                  | Description | Type                                          |
| ---------------------- | ----------- | --------------------------------------------- |
| `expandedStateChanged` |             | `CustomEvent<ActivityPickerStateChangedArgs>` |


## Dependencies

### Used by

 - [elsa-workflow-editor](../elsa-workflow-editor)

### Depends on

- [elsa-activity](../elsa-activity)

### Graph
```mermaid
graph TD;
  elsa-activity-picker --> elsa-activity
  elsa-workflow-editor --> elsa-activity-picker
  style elsa-activity-picker fill:#f9f,stroke:#333,stroke-width:4px
```

----------------------------------------------

*Built with [StencilJS](https://stenciljs.com/)*
