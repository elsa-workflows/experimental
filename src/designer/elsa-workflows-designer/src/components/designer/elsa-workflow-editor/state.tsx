import {createProviderConsumer} from "@stencil/state-tunnel";
import {h} from "@stencil/core";
import {ActivityDescriptor} from "../../../models";

export interface WorkflowEditorState {
  workflowDefinitionId: string;
  activityDescriptors: Array<ActivityDescriptor>;
}

export default createProviderConsumer<WorkflowEditorState>(
  {
    workflowDefinitionId: null,
    activityDescriptors: []
  },
  (subscribe, child) => (<context-consumer subscribe={subscribe} renderer={child}/>)
);
