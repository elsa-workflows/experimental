import {Component, Prop, h} from '@stencil/core';
import {camelCase} from 'lodash'
import {RenderActivityPropContext} from '../designer/activity-properties-editor/activity-properties-editor';
import {ActivityInput, LiteralExpression} from "../../models";

@Component({
  tag: 'elsa-single-line-input',
  shadow: false
})
export class SingleLineInput {
  @Prop() renderContext: RenderActivityPropContext;

  public render() {
    const renderContext = this.renderContext;
    const activity = this.renderContext.activity;
    const inputProperty = renderContext.inputDescriptor;
    const propertyName = inputProperty.name;
    const camelCasePropertyName = camelCase(propertyName);
    const fieldName = inputProperty.name;
    const fieldId = inputProperty.name;
    const input = activity[camelCasePropertyName] as ActivityInput;
    const value = (input?.expression as LiteralExpression)?.value;
    return <input type="text" name={fieldName} id={fieldId} value={value} onChange={e => this.onPropertyEditorChanged(e, propertyName)}/>
  }

  private onPropertyEditorChanged(e: Event, propertyName: string) {

  }
}
