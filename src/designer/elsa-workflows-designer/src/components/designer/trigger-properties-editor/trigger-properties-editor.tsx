import {Component, Event, EventEmitter, h, Method, Prop, State} from '@stencil/core';
import {camelCase} from 'lodash';
import WorkflowEditorTunnel from '../state';
import {
  ActionDefinition,
  Activity,
  ActivityDescriptor,
  ActivityInput,
  DefaultActions,
  LiteralExpression, TabChangedArgs,
  TabDefinition, Trigger, TriggerDescriptor
} from '../../../models';

export interface TriggerUpdatedArgs {
  trigger: Trigger;
}

export interface DeleteTriggerRequestedArgs {
  trigger: Trigger;
}

@Component({
  tag: 'elsa-trigger-properties-editor',
})
export class TriggerPropertiesEditor {
  private slideOverPanel: HTMLElsaSlideOverPanelElement;
  private inputPropertiesContainer: HTMLElement;

  @Prop({mutable: true}) triggerDescriptors: Array<TriggerDescriptor> = [];
  @Prop({mutable: true}) trigger?: Trigger;

  @Event() triggerUpdated: EventEmitter<TriggerUpdatedArgs>;
  @Event() deleteTriggerRequested: EventEmitter<DeleteTriggerRequestedArgs>;

  @State() private selectedTabIndex: number = 0;

  @Method()
  public async show(): Promise<void> {
    await this.slideOverPanel.show();
  }

  @Method()
  public async hide(): Promise<void> {
    await this.slideOverPanel.hide();
  }

  public render() {
    const trigger = this.trigger;
    const descriptor = this.findDescriptor();
    const title = descriptor?.displayName ?? descriptor?.triggerType ?? 'Unknown Trigger';

    const propertiesTab: TabDefinition = {
      displayText: 'Properties',
      content: () => this.renderPropertiesTab(trigger, descriptor)
    };

    const commonTab: TabDefinition = {
      displayText: 'Common',
      content: () => this.renderCommonTab(trigger, descriptor)
    };

    const tabs = !!descriptor ? [propertiesTab, commonTab] : [];
    const actions = [DefaultActions.Delete(this.onDeleteTrigger)];

    return (
      <elsa-form-panel
        headerText={title} tabs={tabs} selectedTabIndex={this.selectedTabIndex}
        onSelectedTabIndexChanged={e => this.onSelectedTabIndexChanged(e)}
        actions={actions}/>
    );
  }

  private findDescriptor = (): TriggerDescriptor => !!this.trigger ? this.triggerDescriptors.find(x => x.triggerType == this.trigger.triggerType) : null;

  private onSelectedTabIndexChanged(e: CustomEvent<TabChangedArgs>) {
    this.selectedTabIndex = e.detail.selectedTabIndex;
  }

  private onTriggerIdChanged(e: any) {
    const trigger = this.trigger;
    const inputElement = e.target as HTMLInputElement;

    trigger.id = inputElement.value;
    this.triggerUpdated.emit({trigger});
  }

  private onPropertyEditorChanged(e: any, propertyName: string) {
    const trigger = this.trigger;
    const inputElement = e.target as HTMLInputElement;
    const value = inputElement.value;
    const camelCasePropertyName = camelCase(propertyName);

    trigger[camelCasePropertyName] = {
      type: 'string',
      expression: {
        type: 'Literal',
        value: value
      }
    };

    this.triggerUpdated.emit({trigger});
  }

  private onDeleteTrigger = (e: any, action: ActionDefinition) => this.deleteTriggerRequested.emit({trigger: this.trigger});

  private renderPropertiesTab(trigger: Trigger, descriptor: TriggerDescriptor) {

    const inputProperties = descriptor.inputProperties;

    return <div>
      {inputProperties.map(inputProperty => {
        const propertyName = inputProperty.name;
        const camelCasePropertyName = camelCase(propertyName);
        const displayName = inputProperty.displayName || propertyName;
        const description = inputProperty.description;
        const fieldName = inputProperty.name;
        const fieldId = inputProperty.name;
        const input = trigger[camelCasePropertyName] as ActivityInput;
        const value = (input?.expression as LiteralExpression)?.value;
        const key = `${trigger.id}_${propertyName}`;

        return <div class="p-4" ref={el => this.inputPropertiesContainer = el}>
          <label htmlFor={fieldId} class="block text-sm font-medium text-gray-700">
            {displayName}
          </label>
          <div class="mt-1">
            <input key={key} type="text" name={fieldName} id={fieldId} value={value}
                   onChange={e => this.onPropertyEditorChanged(e, propertyName)}
                   class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
          </div>
          {description ? <p class="mt-2 text-sm text-gray-500">{description}</p> : undefined}
        </div>
      })}
    </div>
  }

  private renderCommonTab(trigger: Trigger, descriptor: TriggerDescriptor) {

    const triggerId = trigger.id;

    return <div>
      <div class="p-4">
        <label htmlFor="TriggerId" class="block text-sm font-medium text-gray-700">
          ID
        </label>
        <div class="mt-1">
          <input type="text" name="TriggerId" id="TriggerId" value={triggerId}
                 onChange={e => this.onTriggerIdChanged(e)}
                 class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">The ID of the trigger.</p>
      </div>
    </div>
  }
}

WorkflowEditorTunnel.injectProps(TriggerPropertiesEditor, ['triggerDescriptors']);
