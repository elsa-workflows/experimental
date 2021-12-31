import {Type} from "./shared";
import {Expression} from "./expressions";
import {VersionOptions} from "./api";

export interface Node {
  id: string;
}

export interface Trigger extends Node {
  triggerType: string;
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
  identity: WorkflowIdentity;
  publication: WorkflowPublication;
  metadata: WorkflowMetadata;
  root: Activity;
  triggers: Array<Trigger>;
}

export interface WorkflowMetadata {
  name?: string;
  description?: string;
  createdAt?: Date;
}

export interface WorkflowIdentity {
  id: string;
  definitionId: string;
  version: number;
}

export interface WorkflowPublication {
  isLatest: boolean;
  isPublished: boolean;
}

export interface WorkflowState {
  id: string;
  activityOutput: Map<string, Map<string, any>>;
  completionCallbacks: Array<CompletionCallbackState>;
  activityExecutionContexts: Array<ActivityExecutionContextState>;
  properties: Map<string, any>;
}

export interface CompletionCallbackState {
  ownerId: string;
  childId: string;
  methodName: string;
}

export interface ActivityExecutionContextState {
  id: string;
  parentActivityExecutionContextId?: string;
  scheduledActivityId: string;
  ownerActivityId?: string;
  properties: Map<string, any>;
  register: RegisterState;
}

export interface RegisterState {
  locations: Map<string, RegisterLocation>;
}

export interface RegisterLocation {
  value: any;
}

export const getVersionOptionsString = (versionOptions?: VersionOptions) => {

  if (!versionOptions)
    return '';

  return versionOptions.allVersions
    ? 'AllVersions'
    : versionOptions.isDraft
      ? 'Draft'
      : versionOptions.isLatest
        ? 'Latest'
        : versionOptions.isPublished
          ? 'Published'
          : versionOptions.isLatestOrPublished
            ? 'LatestOrPublished'
            : versionOptions.version.toString();
};
