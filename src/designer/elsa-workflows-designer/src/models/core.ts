import {Type} from "./shared";
import {Expression} from "./expressions";

export interface Node {
  id: string;
}

export interface Trigger extends Node {
}

export interface Activity extends Node {
  activityType: string;
  metadata: any;
  input: Map<string, ActivityInput>;
}

export interface Container extends Node {
  activities: Array<Activity>;
  variables: Array<Variable>;
}

export interface Variable {
  name: string;
  defaultValue?: any;
}

export interface ActivityInput {
  type: Type;
  expression: Expression;
}
