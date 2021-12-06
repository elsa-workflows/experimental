import {createProviderConsumer} from "@stencil/state-tunnel";
import {h} from "@stencil/core";
import {ActivityDescriptor, TriggerDescriptor} from "../../../models";

export interface WorkflowEditorState {
  workflowDefinitionId: string;
  activityDescriptors: Array<ActivityDescriptor>;
  triggerDescriptors: Array<TriggerDescriptor>;
}

export default createProviderConsumer<WorkflowEditorState>(
  {
    workflowDefinitionId: null,
    activityDescriptors: [],
    triggerDescriptors: []
  },
  (subscribe, child) => (<context-consumer subscribe={subscribe} renderer={child}/>)
);
