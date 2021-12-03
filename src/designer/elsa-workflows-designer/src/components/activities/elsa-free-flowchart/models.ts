import {Activity, Container} from "../../../models";

export interface FreeFlowchart extends Container {
  start: Activity;
  connections: Array<Connection>;
}

export interface Connection {
  source: Activity;
  target: Activity;
  outboundPort: string;
}
