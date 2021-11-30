import {Component, Event, EventEmitter, h, Method, Prop} from "@stencil/core";
import WorkflowEditorTunnel from "./state";
import {ActionDefinition, ActionType, DefaultActions, TabDefinition} from "../elsa-slide-over-panel/models";
import {Activity, ActivityDescriptor, ActivityInput, LiteralExpression} from "../../../models";

@Component({
  tag: 'elsa-activity-properties-editor',
})
export class ElsaActivityPropertiesEditor {
  private slideOverPanel: HTMLElsaSlideOverPanelElement;

  @Prop({mutable: true}) activityDescriptors: Array<ActivityDescriptor> = [];
  @Prop({mutable: true}) activity?: Activity;

  @Event() activityUpdated: EventEmitter<Activity>;

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
      content: () => ElsaActivityPropertiesEditor.renderPropertiesTab(activity, activityDescriptor)
    };

    const commonTab: TabDefinition = {
      displayText: 'Common',
      content: () => ElsaActivityPropertiesEditor.renderCommonTab(activity, activityDescriptor)
    };

    const tabs = !!activityDescriptor ? [propertiesTab, commonTab] : [];
    const expanded = !!activity;
    const selectedTab = tabs.length > 0 ? tabs[0] : null;
    const actions = [DefaultActions.Cancel, DefaultActions.Save];

    return (
      <elsa-slide-over-panel expand={expanded} headerText={title} tabs={tabs} selectedTab={selectedTab}
                             actions={actions}
                             ref={el => this.slideOverPanel = el}
                             onCollapsed={() => this.onPanelCollapsed()}
                             onSubmitted={e => this.onSubmit(e)}
      />
    );
  }

  private findActivityDescriptor = (): ActivityDescriptor => !!this.activity ? this.activityDescriptors.find(x => x.activityType == this.activity.activityType) : null;
  private onPanelCollapsed = () => this.activity = null;

  private onSubmit = (e: CustomEvent<FormData>) => {
    const activity = {...this.activity}; // Copy activity.
    const activityDescriptor = this.findActivityDescriptor();
    const inputProperties = activityDescriptor.inputProperties;
    const formData = e.detail;

    // Update common properties.
    activity.id = formData.get('ActivityId').toString();

    // Update each activity input property.
    for (const inputProperty of inputProperties) {
      const propertyName = inputProperty.name;
      const propertyValue = formData.get(propertyName);

      const input: ActivityInput = {
        type: 'string',
        expression: {
          type: 'LiteralExpression',
          value: propertyValue
        }
      };

      activity.input.set(propertyName, input);
    }

    // Publish event.
    this.activityUpdated.emit(activity);

    // Close editor.
    this.activity = null;
  };

  private static renderPropertiesTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    const inputProperties = activityDescriptor.inputProperties;

    return <div>
      {inputProperties.map(inputProperty => {
        const propertyName = inputProperty.name;
        const displayName = inputProperty.displayName || propertyName;
        const description = inputProperty.description;
        const fieldName = inputProperty.name;
        const fieldId = inputProperty.name;
        const input = activity.input.get(propertyName) as ActivityInput;

        const value = (input?.expression as LiteralExpression)?.value;

        return <div class="p-4">
          <label htmlFor={fieldId} class="block text-sm font-medium text-gray-700">
            {displayName}
          </label>
          <div class="mt-1">
            <input type="text" name={fieldName} id={fieldId} value={value}
                   class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
          </div>
          {description ? <p class="mt-2 text-sm text-gray-500">{description}</p> : undefined}
        </div>
      })}
    </div>
  }

  private static renderCommonTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    const activityId = activity.id;

    return <div>
      <div class="p-4">
        <label htmlFor="ActivityId" class="block text-sm font-medium text-gray-700">
          ID
        </label>
        <div class="mt-1">
          <input type="text" name="ActivityId" id="ActivityId" value={activityId}
                 class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">The ID of the activity.</p>
      </div>
    </div>
  }
}

WorkflowEditorTunnel.injectProps(ElsaActivityPropertiesEditor, ['activityDescriptors']);
