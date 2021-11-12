import {AddActivityArgs} from "../designer/elsa-canvas/elsa-canvas";

export interface ActivityComponent {
  updateLayout(): Promise<void>;
  addActivity(args: AddActivityArgs): Promise<void>;
}
