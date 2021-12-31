import {Activity, ActivityDescriptor, InputDescriptor} from "../models";

export interface RenderInputContext {
  activity: Activity;
  activityDescriptor: ActivityDescriptor;
  inputDescriptor: InputDescriptor;
}

export interface InputDriver {
  supportsInput(context: RenderInputContext): boolean;

  get priority(): number;

  renderInput(context: RenderInputContext): any;
}
