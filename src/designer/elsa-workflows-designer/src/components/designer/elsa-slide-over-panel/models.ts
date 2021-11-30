export interface TabDefinition {
  displayText: string;
  content: () => any;
}

export enum ActionType {
  Button,
  Submit,
  Cancel
}

export interface ActionDefinition {
  text: string;
  name?: string;
  isPrimary?: boolean;
  type?: ActionType;
  onClick?: (e: Event, action: ActionDefinition) => void;
  display?: (button: ActionDefinition) => any;
}

export class DefaultActions{

  public static Cancel: ActionDefinition = {
    text:'Cancel',
    type: ActionType.Cancel
  };

  public static Save: ActionDefinition = {
    text: 'Save',
    type: ActionType.Submit,
    isPrimary: true,
  };
}
