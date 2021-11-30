export interface ActivityDescriptorResponse {
  activityDescriptors: Array<ActivityDescriptor>;
}

export interface ActivityDescriptor {
  activityType: string;
  displayName: string;
  category: string;
  kind: ActivityKind;
  inputProperties: Array<ActivityInputDescriptor>
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

export interface ActivityPropertyDescriptor {
  name: string;
  type: DotNetType;
  displayName?: string;
  description?: string;
  order: number;
  isBrowsable?: boolean;
}

export interface ActivityInputDescriptor extends ActivityPropertyDescriptor {
  uiHint: string;
  options?: any;
  category?: string;
  defaultValue?: any;
  defaultSyntax?: string;
  supportedSyntaxes?: Array<string>;
  isReadOnly?: boolean;
}

export interface ActivityOutputDescriptor extends ActivityPropertyDescriptor {
}

export type DotNetType = string;
