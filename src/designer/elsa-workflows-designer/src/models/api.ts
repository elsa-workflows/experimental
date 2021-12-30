import {Type} from './shared';

export interface ActivityDescriptorResponse {
  activityDescriptors: Array<ActivityDescriptor>;
}

export interface ActivityDescriptor {
  activityType: string;
  displayName: string;
  category: string;
  traits: ActivityTraits;
  inputProperties: Array<InputDescriptor>
  inPorts: Array<Port>;
  outPorts: Array<Port>;
}

export interface TriggerDescriptorResponse {
  triggerDescriptors: Array<TriggerDescriptor>;
}

export interface TriggerDescriptor {
  triggerType: string;
  displayName: string;
  category: string;
  inputProperties: Array<InputDescriptor>;
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

export interface InputDescriptor extends ActivityPropertyDescriptor {
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

export interface Port {
  name: string;
  displayName: string;
}

export interface WorkflowSummary {
  id: string;
  definitionId: string;
  version: number;
  name?: string;
  isPublished: boolean;
  isLatest: boolean;
}

export enum WorkflowStatus {
  Idle = 'Idle',
  Running = 'Running',
  Suspended = 'Suspended',
  Finished = 'Finished',
  Compensating = 'Compensating',
  Cancelled = 'Cancelled',
  Faulted = 'Faulted',
}

export enum OrderBy {
  Created = 'Created',
  LastExecuted = 'LastExecuted',
  Finished = 'Finished'
}

export enum OrderDirection {
  Ascending = 'Ascending',
  Descending = 'Descending'
}

export interface WorkflowInstanceSummary {
  id: string;
  definitionId: string;
  definitionVersionId: string;
  version: number;
  workflowStatus: WorkflowStatus;
  correlationId: string;
  name?: string;
  createdAt: Date;
  lastExecutedAt?: Date;
  finishedAt?: Date;
  cancelledAt?: Date;
  faultedAt?: Date;
}

export interface PagedList<T> {
  items: Array<T>;
  page?: number;
  pageSize?: number;
  totalCount: number;
}

export interface VersionOptions {
  isLatest?: boolean;
  isLatestOrPublished?: boolean;
  isPublished?: boolean;
  isDraft?: boolean;
  allVersions?: boolean;
  version?: number;
}
