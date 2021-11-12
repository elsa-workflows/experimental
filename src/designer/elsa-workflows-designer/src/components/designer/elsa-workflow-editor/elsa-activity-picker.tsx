import {Component, FunctionalComponent, h, Prop, State, Watch} from "@stencil/core";
import {Addon, Graph} from '@antv/x6';
import groupBy from 'lodash/groupBy';
import {ActivityDescriptor, ActivityKind} from '../../../models';
import {ActivityDescriptorView} from "../elsa-activity-descriptor/elsa-activity-descriptor-view";
import WorkflowEditorTunnel, {WorkflowEditorState} from "./state";

@Component({
  tag: 'elsa-activity-picker',
})
export class ElsaActivityPicker {
  @Prop() graph: Graph;
  @Prop() activityDescriptors: Array<ActivityDescriptor> = [];
  private dnd: Addon.Dnd;

  @Watch('graph')
  handleGraphChanged(value: Graph) {

    if (this.dnd)
      this.dnd.dispose();

    this.dnd = new Addon.Dnd({
      target: value,
      scaled: false,
      animation: true,
    });
  }

  private static onStartDrag(e: DragEvent, activity: ActivityDescriptor) {
    const json = JSON.stringify(activity);
    const isTrigger = activity.kind == ActivityKind.Trigger;
    e.dataTransfer.setData('activity-descriptor', json);

    if (isTrigger) {
      e.dataTransfer.setData('trigger-descriptor', json);
    }
  }

  render() {
    const activityDescriptors = this.activityDescriptors;
    const categorizedActivitiesLookup = groupBy(activityDescriptors, x => x.category);
    const categories = Object.keys(categorizedActivitiesLookup);

    return (

      <div class="activity-list">

        <div class="border-b border-gray-200">
          <nav class="-mb-px flex" aria-label="Tabs">
            <a href="#"
               class="border-blue-500 text-blue-600 w-1/2 py-4 px-1 text-center border-b-2 font-medium text-sm">
              Activities
            </a>

            <a href="#"
               class="border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 w-1/2 py-4 px-1 text-center border-b-2 font-medium text-sm">
              Triggers
            </a>
          </nav>
        </div>

        <nav class="flex-1 px-2 space-y-1 font-sans text-sm text-gray-600">
          {categories.map(category => {

              const activities = categorizedActivitiesLookup[category] as ActivityDescriptor[];

              return <div class="space-y-1">
                <button type="button"
                        class="text-gray-600 hover:bg-gray-50 hover:text-gray-900 group w-full flex items-center pr-2 py-2 text-left text-sm font-medium rounded-md focus:outline-none">
                  <svg
                    class="text-gray-300 mr-2 flex-shrink-0 h-5 w-5 transform group-hover:text-gray-400 transition-colors ease-in-out duration-150"
                    viewBox="0 0 20 20" aria-hidden="true">
                    <path d="M6 6L14 10L6 14V6Z" fill="currentColor"/>
                  </svg>
                  {category}
                </button>

                <div class="space-y-1"
                     id="sub-menu-1" style={{display: "block"}}>

                  {activities.map(activity => (
                    <div class="w-full flex items-center pl-10 pr-2 py-2">
                      <div class="cursor-move" onDragStart={e => ElsaActivityPicker.onStartDrag(e, activity)}>
                        <ActivityDescriptorView activityDescriptor={activity}/>
                      </div>
                    </div>
                  ))}

                </div>
              </div>;
            }
          )}

        </nav>
      </div>
    );
  }
}

WorkflowEditorTunnel.injectProps(ElsaActivityPicker, ['activityDescriptors']);
