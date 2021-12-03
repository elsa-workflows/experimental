import {Activity} from "./core";

export interface ActivityEditRequestArgs {
  activity: Activity;
  applyChanges: (activity: Activity) => void;
  deleteActivity: (activity: Activity) => void;
}

export interface GraphUpdatedArgs {
  exportGraph: () => Activity;
}
