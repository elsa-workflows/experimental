import {AddActivityArgs} from "../designer/elsa-canvas/elsa-canvas";
import {Activity} from "../../models";

export interface ContainerActivityComponent {
  updateLayout(): Promise<void>;
  addActivity(args: AddActivityArgs): Promise<void>;
  exportGraph(): Promise<Activity>
}
