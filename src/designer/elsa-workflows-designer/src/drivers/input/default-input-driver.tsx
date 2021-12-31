import {InputDriver, RenderInputContext} from "../../services/input-driver";
import {Container} from "typedi";
import {camelCase} from 'lodash';
import {InputControlRegistry} from "../../services/input-control-registry";
import {Hint} from "../../components/forms/hint";
import {h} from "@stencil/core";
import {Activity, ActivityInput, LiteralExpression} from "../../models";

// A standard input driver that determines the UI to be displayed based on the UI hint.
export class DefaultInputDriver implements InputDriver {
  private inputControlRegistry: InputControlRegistry;

  constructor() {
    this.inputControlRegistry = Container.get(InputControlRegistry);
  }

  get priority(): number {
    return -1;
  }

  renderInput(context: RenderInputContext): any {
    const inputDescriptor = context.inputDescriptor;
    const uiHint = inputDescriptor.uiHint;
    const inputControl = this.inputControlRegistry.get(uiHint);
    const activity = context.activity;
    const propertyName = inputDescriptor.name;
    const camelCasePropertyName = camelCase(propertyName);
    const displayName = inputDescriptor.displayName || propertyName;
    const description = inputDescriptor.description;
    //const fieldName = inputDescriptor.name;
    const fieldId = inputDescriptor.name;
    const input = activity[camelCasePropertyName] as ActivityInput;
    //const value = (input?.expression as LiteralExpression)?.value;
    const key = `${activity.id}_${propertyName}`;

    return (
      <div class="p-4">
        <label htmlFor={fieldId}>
          {displayName}
        </label>
        <div class="mt-1" key={key}>
          {/*<input key={key} type="text" name={fieldName} id={fieldId} value={value} onChange={e => this.onPropertyEditorChanged(e, activity, propertyName)}/>*/}
          {inputControl(context)}
        </div>
        <Hint text={description}/>
      </div>
    );
  }

  supportsInput(context: RenderInputContext): boolean {
    const uiHint = context.inputDescriptor.uiHint;
    return this.inputControlRegistry.has(uiHint);
  }

  private onPropertyEditorChanged = (e: Event, activity: Activity, propertyName: string) => {
    const inputElement = e.target as HTMLInputElement;
    const value = inputElement.value;
    const camelCasePropertyName = camelCase(propertyName);

    activity[camelCasePropertyName] = {
      type: 'string',
      expression: {
        type: 'Literal',
        value: value
      }
    };

    //this.activityUpdated.emit({activity: activity});
  }
}
