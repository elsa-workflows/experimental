import {Component, Event, EventEmitter, h, Method, Prop, State} from "@stencil/core";
import _, {camelCase} from 'lodash';
import WorkflowEditorTunnel from "./state";
import {
  ActionDefinition,
  Activity,
  ActivityDescriptor,
  ActivityInput,
  DefaultActions,
  LiteralExpression, TabChangedArgs,
  TabDefinition
} from "../../../models";

export interface ActivityUpdatedArgs {
  activity: Activity;
}

export interface DeleteActivityRequestedArgs {
  activity: Activity;
}

@Component({
  tag: 'elsa-activity-properties-editor',
})
export class ElsaActivityPropertiesEditor {
  private slideOverPanel: HTMLElsaSlideOverPanelElement;
  private inputPropertiesContainer: HTMLElement;

  @Prop({mutable: true}) activityDescriptors: Array<ActivityDescriptor> = [];
  @Prop({mutable: true}) activity?: Activity;

  @Event() activityUpdated: EventEmitter<ActivityUpdatedArgs>;
  @Event() deleteActivityRequested: EventEmitter<DeleteActivityRequestedArgs>;

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
    const activity = this.activity;
    const activityDescriptor = this.findActivityDescriptor();
    const title = activityDescriptor?.displayName ?? activityDescriptor?.activityType ?? 'Unknown Activity';

    const propertiesTab: TabDefinition = {
      displayText: 'Properties',
      content: () => this.renderPropertiesTab(activity, activityDescriptor)
    };

    const commonTab: TabDefinition = {
      displayText: 'Common',
      content: () => this.renderCommonTab(activity, activityDescriptor)
    };

    const tabs = !!activityDescriptor ? [propertiesTab, commonTab] : [];
    const actions = [DefaultActions.Delete(this.onDeleteActivity)];

    return (
      <elsa-form-panel
        headerText={title} tabs={tabs} selectedTabIndex={this.selectedTabIndex}
        onSelectedTabIndexChanged={e => this.onSelectedTabIndexChanged(e)}
        actions={actions}/>
    );
  }

  public componentDidRender() {

    const firstTextInputElement = this.inputPropertiesContainer?.querySelector('input');

    if (!firstTextInputElement)
      return;

    firstTextInputElement.focus({preventScroll: false});
  }

  private findActivityDescriptor = (): ActivityDescriptor => !!this.activity ? this.activityDescriptors.find(x => x.activityType == this.activity.activityType) : null;

  private onSelectedTabIndexChanged(e: CustomEvent<TabChangedArgs>) {
    this.selectedTabIndex = e.detail.selectedTabIndex;
  }

  private onActivityIdChanged(e: any) {
    const activity = this.activity;
    const inputElement = e.target as HTMLInputElement;

    activity.id = inputElement.value;
    this.activityUpdated.emit({activity: activity});
  }

  private onPropertyEditorChanged(e: any, propertyName: string) {
    const activity = this.activity;
    const inputElement = e.target as HTMLInputElement;
    const value = inputElement.value;
    const camelCasePropertyName = _.camelCase(propertyName);

    activity[camelCasePropertyName] = {
      type: 'string',
      expression: {
        type: 'Literal',
        value: value
      }
    };

    this.activityUpdated.emit({activity: activity});
  }

  private onDeleteActivity = (e: any, action: ActionDefinition) => this.deleteActivityRequested.emit({activity: this.activity});

  private renderPropertiesTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    const inputProperties = activityDescriptor.inputProperties;

    return <div>
      {inputProperties.map(inputProperty => {
        const propertyName = inputProperty.name;
        const camelCasePropertyName = _.camelCase(propertyName);
        const displayName = inputProperty.displayName || propertyName;
        const description = inputProperty.description;
        const fieldName = inputProperty.name;
        const fieldId = inputProperty.name;
        const input = activity[camelCasePropertyName] as ActivityInput;
        const value = (input?.expression as LiteralExpression)?.value;
        const key = `${activity.id}_${propertyName}`;

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

  private renderCommonTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    const activityId = activity.id;

    return <div>
      <div class="p-4">
        <label htmlFor="ActivityId" class="block text-sm font-medium text-gray-700">
          ID
        </label>
        <div class="mt-1">
          <input type="text" name="ActivityId" id="ActivityId" value={activityId}
                 onChange={e => this.onActivityIdChanged(e)}
                 class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">The ID of the activity.</p>
      </div>
    </div>
  }
}

WorkflowEditorTunnel.injectProps(ElsaActivityPropertiesEditor, ['activityDescriptors']);
