import {Component, h, Method, Prop} from "@stencil/core";
import WorkflowEditorTunnel from "./state";
import {TabDefinition} from "../elsa-slide-over-panel/models";
import {Activity, ActivityDescriptor} from "../../../models";

@Component({
  tag: 'elsa-activity-properties-editor',
})
export class ElsaActivityPropertiesEditor {
  private slideOverPanel: HTMLElsaSlideOverPanelElement;

  @Prop() activityDescriptors: Array<ActivityDescriptor> = [];
  @Prop() activity?: Activity;

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
    const activityDescriptor = !!activity ? this.activityDescriptors.find(x => x.activityType == activity.activityType) : null;
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

    return (
      <elsa-slide-over-panel expand={expanded} headerText={title} tabs={tabs} selectedTab={selectedTab}
                             ref={el => this.slideOverPanel = el}
                             onCollapsed={() => this.onPanelCollapsed()}/>
    );
  }

  private static renderPropertiesTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    const inputProperties = activityDescriptor.inputProperties;

    return <div>
      {inputProperties.map(inputProperty => {
        const displayName = inputProperty.displayName || inputProperty.name;
        const description = inputProperty.description;
        const fieldName = inputProperty.name;
        const fieldId = inputProperty.name;

        return <div class="p-4">
          <label htmlFor={fieldId} class="block text-sm font-medium text-gray-700">
            {displayName}
          </label>
          <div class="mt-1">
            <input type="text" name={fieldName} id={fieldId}
                   class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
          </div>
          {description ? <p class="mt-2 text-sm text-gray-500">{description}</p> : undefined}
        </div>
      })}
    </div>
  }

  private static renderCommonTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    return <div>
      <div class="p-4">
        <label htmlFor="ActivityName" class="block text-sm font-medium text-gray-700">
          Name
        </label>
        <div class="mt-1">
          <input type="text" name="ActivityName" id="ActivityName"
                 class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">The technical name of the activity.</p>
      </div>
    </div>
  }

  private onPanelCollapsed = () => {
    this.activity = null;
  };
}

WorkflowEditorTunnel.injectProps(ElsaActivityPropertiesEditor, ['activityDescriptors']);
