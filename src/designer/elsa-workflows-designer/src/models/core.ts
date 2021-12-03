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

  [name: string]: any;
}

export interface Container extends Activity {
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

export interface Workflow {
  metadata: WorkflowMetadata;
  root: Activity;
  triggers: Array<Trigger>;
}

export interface WorkflowMetadata {
  identity: WorkflowIdentity;
  publication: WorkflowPublication;
}

export interface WorkflowIdentity {
  id: string;
  version: number;
}

export interface WorkflowPublication {
  isLatest: boolean;
  isPublished: boolean;
}
