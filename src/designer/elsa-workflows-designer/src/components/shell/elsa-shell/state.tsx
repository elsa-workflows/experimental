import {createProviderConsumer} from "@stencil/state-tunnel";
import {h} from "@stencil/core";
import {ActivityDescriptor} from "../../../models";

export interface ShellState {
  activityDescriptors: Array<ActivityDescriptor>;
}

export default createProviderConsumer<ShellState>(
  {
    activityDescriptors: []
  },
  (subscribe, child) => (<context-consumer subscribe={subscribe} renderer={child}/>)
);
