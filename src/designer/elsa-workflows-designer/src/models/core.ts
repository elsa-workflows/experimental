export interface Node {
  id: string;
}

export interface Trigger extends Node {
}

export interface Activity extends Node {
  activityType: string;
  metadata: any;
}

export interface Container extends Node {
  activities: Array<Activity>;
  variables: Array<Variable>;
}

export interface Variable {
  name: string;
  defaultValue?: any;
}
