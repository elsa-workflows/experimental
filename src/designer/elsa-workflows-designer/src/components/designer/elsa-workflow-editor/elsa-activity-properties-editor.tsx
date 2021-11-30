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
      content: () => this.renderPropertiesTab(activity, activityDescriptor)
    };

    const tabs = !!activityDescriptor ? [propertiesTab] : [];
    const expanded = !!activity;

    return (
      <elsa-slide-over-panel expand={expanded} headerText={title} tabs={tabs} ref={el => this.slideOverPanel = el}
                             onCollapsed={() => this.onPanelCollapsed()}/>
    );
  }

  private renderPropertiesTab(activity: Activity, activityDescriptor: ActivityDescriptor) {

    const inputProperties = activityDescriptor.inputProperties;

    return <div>
      {inputProperties.map(inputProperty => {
        const displayName = inputProperty.displayName || inputProperty.name;
        return <div class="p-4">
          <label htmlFor="activity-name" class="block text-sm font-medium text-gray-700">
            {displayName}
          </label>
          <div class="mt-1">
            <input type="text" name="activity-name" id="activity-name"
                   class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
          </div>
          {inputProperty.description ? <p class="mt-2 text-sm text-gray-500">The name of the activity.</p> : undefined}
        </div>
      })}
    </div>
  }

  private onPanelCollapsed = () => {
    this.activity = null;
  };
}

WorkflowEditorTunnel.injectProps(ElsaActivityPropertiesEditor, ['activityDescriptors']);
