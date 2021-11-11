export interface ActivityDescriptorResponse {
  activityDescriptors: Array<ActivityDescriptor>;
}

export interface ActivityDescriptor {
  activityType: string;
  displayName: string;
  category: string;
  kind: ActivityKind;
}

export interface TriggerDescriptor {
  triggerType: string;
  displayName: string;
  category: string;
}

export enum ActivityKind {
  Action,
  Trigger
}
