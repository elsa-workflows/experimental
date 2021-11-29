import {Component, FunctionalComponent, h, Method, Prop, State, Watch} from "@stencil/core";
import {Addon, Graph} from '@antv/x6';
import groupBy from 'lodash/groupBy';
import {ActivityDescriptor, ActivityKind} from '../../../models';
import {ActivityDescriptorView} from "../elsa-activity-descriptor/elsa-activity-descriptor-view";
import WorkflowEditorTunnel, {WorkflowEditorState} from "./state";
import {TabDefinition} from "../elsa-slide-over-panel/models";

@Component({
  tag: 'elsa-activity-properties-editor',
})
export class ElsaActivityPropertiesEditor {
  private slideOverPanel: HTMLElsaSlideOverPanelElement;

  @Method()
  public async show(): Promise<void> {
    await this.slideOverPanel.show();
  }

  @Method()
  public async hide(): Promise<void> {
    await this.slideOverPanel.hide();
  }

  render() {

    const propertiesTab: TabDefinition = {
      displayText: 'Properties',
      content: this.renderPropertiesTab
    };

    const tabs = [propertiesTab];

    return (
      <elsa-slide-over-panel headerText={'Write Line'} tabs={tabs} ref={el => this.slideOverPanel = el}/>
    );
  }

  renderPropertiesTab() {
    return <div>
      <div class="p-4">
        <label htmlFor="activity-name" class="block text-sm font-medium text-gray-700">
          Name
        </label>
        <div class="mt-1">
          <input type="text" name="activity-name" id="activity-name"
                 class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">The name of the activity.</p>
      </div>

      <div class="p-4">
        <label htmlFor="activity-description" class="block text-sm font-medium text-gray-700">
          Description
        </label>
        <div class="mt-1">
                        <textarea name="activity-description" id="activity-description"
                                  class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">A description for this activity.</p>
      </div>
    </div>
  }
}

//WorkflowEditorTunnel.injectProps(ElsaActivityPicker, ['activityDescriptors']);
