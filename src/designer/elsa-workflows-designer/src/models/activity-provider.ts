import {Activity} from "./core";
import {ActivityDescriptor} from "./api";

interface RenderActivityContext {
  activity: Activity;
  activityDescriptor: ActivityDescriptor;
}

export interface ActivityProvider {
  renderOnCanvas: (context: RenderActivityContext) => any;
  renderOnPicker: (context: RenderActivityContext) => any;
}
