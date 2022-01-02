import {Component, Prop, h} from '@stencil/core';
import {camelCase} from 'lodash'
import {ActivityInput, LiteralExpression} from "../../models";
import {NodeInputContext} from "../../services/node-input-driver";

@Component({
  tag: 'elsa-single-line-input',
  shadow: false
})
export class SingleLineInput {
  @Prop() inputContext: NodeInputContext;

  public render() {
    //const renderContext = this.renderContext;
    const inputContext = this.inputContext;
    const node = inputContext.node;
    const inputProperty = inputContext.inputDescriptor;
    const propertyName = inputProperty.name;
    const camelCasePropertyName = camelCase(propertyName);
    const fieldName = inputProperty.name;
    const fieldId = inputProperty.name;
    const input = node[camelCasePropertyName] as ActivityInput;
    const value = (input?.expression as LiteralExpression)?.value;
    return <input type="text" name={fieldName} id={fieldId} value={value} onChange={this.onPropertyEditorChanged}/>
  }

  private onPropertyEditorChanged = (e: Event) => {
    const inputElement = e.target as HTMLInputElement;
    this.inputContext.inputChanged(inputElement.value);
  }
}
