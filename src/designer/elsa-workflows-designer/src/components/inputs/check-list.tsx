import {Component, Prop, h, State} from '@stencil/core';
import {camelCase} from 'lodash'
import {ActivityInput, LiteralExpression, SelectList, SelectListItem} from "../../models";
import {NodeInputContext} from "../../services/node-input-driver";
import {parseJson} from "../../utils/utils";

@Component({
  tag: 'elsa-check-list-input',
  shadow: false
})
export class CheckList {
  @Prop() inputContext: NodeInputContext;
  @State() currentValue?: string;

  public render() {
    const inputContext = this.inputContext;
    const node = inputContext.node;
    const inputProperty = inputContext.inputDescriptor;
    const propertyName = inputProperty.name;
    const camelCasePropertyName = camelCase(propertyName);
    const fieldName = inputProperty.name;
    const fieldId = inputProperty.name;
    const input = node[camelCasePropertyName] as ActivityInput;
    const value = (input?.expression as LiteralExpression)?.value;

    const selectList: SelectList = {
      items: [
        {text: 'Item 1', value: 'item1'},
        {text: 'Item 2', value: 'item2'},
        {text: 'Item 3', value: 'item3'},
      ],
      isFlagsEnum: false
    };

    const selectedValues = selectList.isFlagsEnum ? this.currentValue : parseJson(this.currentValue as string) || [];

    return (
      <div class="elsa-max-w-lg elsa-space-y-3 elsa-my-4">
        {selectList.items.map((item, index) => {
          const inputId = `${fieldId}_${index}`;
          const optionIsString = typeof (item as any) == 'string';
          const value = optionIsString ? item : item.value;
          const text = optionIsString ? item : item.text;
          const isSelected = selectList.isFlagsEnum
            ? ((parseInt(this.currentValue)) & (parseInt(value as string))) == parseInt(value as string)
            : selectedValues.findIndex(x => x == value) >= 0;

          return (
            <div class="elsa-relative elsa-flex elsa-items-start">
              <div class="elsa-flex elsa-items-center elsa-h-5">
                <input id={inputId} type="checkbox" checked={isSelected} value={value}
                       onChange={e => this.onCheckChanged(e)}
                       class="focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"/>
              </div>
              <div class="elsa-ml-3 elsa-mt-1 elsa-text-sm">
                <label htmlFor={inputId} class="elsa-font-medium elsa-text-gray-700">{text}</label>
              </div>
            </div>
          );
        })}
      </div>
    );
  }

  private onCheckChanged = (e: Event) => {
    const inputElement = e.target as HTMLInputElement;
    this.inputContext.inputChanged(inputElement.value);
  }
}
