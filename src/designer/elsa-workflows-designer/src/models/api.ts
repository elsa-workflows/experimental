import {Type} from "./shared";

export interface ActivityDescriptorResponse {
  activityDescriptors: Array<ActivityDescriptor>;
}

export interface ActivityDescriptor {
  activityType: string;
  displayName: string;
  category: string;
  traits: ActivityTraits;
  inputProperties: Array<ActivityInputDescriptor>
}

export interface TriggerDescriptor {
  triggerType: string;
  displayName: string;
  category: string;
}

export enum ActivityTraits {
  Action = 1,
  Trigger = 2
}

export interface ActivityPropertyDescriptor {
  name: string;
  type: Type;
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
