import {createProviderConsumer} from "@stencil/state-tunnel";
import {h} from "@stencil/core";
import {ActivityDescriptor, TriggerDescriptor} from "../../../models";

export interface ShellState {
  activityDescriptors: Array<ActivityDescriptor>;
  triggerDescriptors: Array<TriggerDescriptor>;
}

export default createProviderConsumer<ShellState>(
  {
    activityDescriptors: [],
    triggerDescriptors: []
  },
  (subscribe, child) => (<context-consumer subscribe={subscribe} renderer={child}/>)
);
