import {Activity} from "./core";

export interface ActivityEditRequestArgs {
  activity: Activity;
  applyChanges: (activity: Activity) => void;
}
