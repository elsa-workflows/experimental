import {Component, h, Prop, State} from "@stencil/core";
import {Addon, Graph} from '@antv/x6';
import {ActivityDescriptor, TriggerDescriptor} from '../../../models';
import WorkflowEditorTunnel from '../elsa-workflow-editor/state';

@Component({
  tag: 'elsa-toolbox',
})
export class ElsaToolbox {
  @Prop() graph: Graph;
  @Prop({mutable: true}) activityDescriptors: Array<ActivityDescriptor> = [];
  @Prop({mutable: true}) triggerDescriptors: Array<TriggerDescriptor> = [];
  @State() selectedTabIndex: number = 0;

  private onTabSelected = (e: Event, index: number) => {
    e.preventDefault();
    this.selectedTabIndex = index;
  };

  render() {

    const selectedTabIndex = this.selectedTabIndex;
    const selectedCss = 'border-blue-500 text-blue-600';
    const defaultCss = 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300';
    const activitiesTabCssClass = selectedTabIndex == 0 ? selectedCss : defaultCss;
    const triggersTabCssClass = selectedTabIndex == 1 ? selectedCss : defaultCss;

    return (

      <div class="activity-list">

        <div class="border-b border-gray-200">
          <nav class="-mb-px flex" aria-label="Tabs">
            <a href="#"
               onClick={e => this.onTabSelected(e, 0)}
               class={`${activitiesTabCssClass} w-1/2 py-4 px-1 text-center border-b-2 font-medium text-sm`}>
              Activities
            </a>

            <a href="#"
               onClick={e => this.onTabSelected(e, 1)}
               class={`${triggersTabCssClass} w-1/2 py-4 px-1 text-center border-b-2 font-medium text-sm`}>
              Triggers
            </a>
          </nav>
        </div>

        <elsa-toolbox-activities graph={this.graph} activityDescriptors={this.activityDescriptors}
                                 class={selectedTabIndex == 0 ? '' : 'hidden'}/>

        <elsa-toolbox-triggers graph={this.graph} triggerDescriptors={this.triggerDescriptors}
                               class={selectedTabIndex == 1 ? '' : 'hidden'}/>
      </div>
    );
  }
}

WorkflowEditorTunnel.injectProps(ElsaToolbox, ['activityDescriptors', 'triggerDescriptors']);
