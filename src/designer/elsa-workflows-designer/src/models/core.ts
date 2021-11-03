export interface Node {
  id: string;
}

export interface Trigger extends Node {
}

export interface Activity extends Node {
  activityType: string;
}
