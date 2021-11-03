export interface ActivityDescriptor {
  activityType: string;
  displayName: string;
  category: string;
  kind: ActivityKind;
}

export enum ActivityKind {
  Action,
  Trigger
}
