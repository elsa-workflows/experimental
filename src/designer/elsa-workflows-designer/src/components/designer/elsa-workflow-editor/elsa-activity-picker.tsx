import {Component, h, Prop, State, Watch} from "@stencil/core";
import {Addon, Graph} from '@antv/x6';
import groupBy from 'lodash/groupBy';
import {ActivityDescriptor, ActivityTraits, TriggerDescriptor} from '../../../models';
import WorkflowEditorTunnel from "./state";
import {TriggerDescriptorView} from "./elsa-trigger-descriptor-view";
import {ActivityDescriptorView} from "./elsa-activity-descriptor-view";

interface CategoryModelBase {
  category: string;
  expanded: boolean;
}

interface CategoryModel<T> extends CategoryModelBase {
  elements: Array<T>;
}

@Component({
  tag: 'elsa-activity-picker',
})
export class ElsaActivityPicker {
  @Prop() graph: Graph;
  @Prop({mutable: true}) activityDescriptors: Array<ActivityDescriptor> = [];
  @Prop({mutable: true}) triggerDescriptors: Array<TriggerDescriptor> = [];
  @State() activityCategoryModels: Array<CategoryModel<ActivityDescriptor>> = [];
  @State() triggerCategoryModels: Array<CategoryModel<TriggerDescriptor>> = [];
  @State() selectedTabIndex: number = 0;
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

  @Watch('activityDescriptors')
  handleActivityDescriptorsChanged(value: Array<ActivityDescriptor>) {
    const categorizedActivitiesLookup = groupBy(value, x => x.category);
    const categories = Object.keys(categorizedActivitiesLookup);

    this.activityCategoryModels = categories.map(x => {
      const model: CategoryModel<ActivityDescriptor> = {
        category: x,
        expanded: false,
        elements: categorizedActivitiesLookup[x]
      };

      return model;
    });
  }

  componentDidLoad() {
    this.handleActivityDescriptorsChanged(this.activityDescriptors);
  }

  private onTabSelected = (e: Event, index: number) => {
    e.preventDefault();
    this.selectedTabIndex = index;
  };

  private static onActivityStartDrag(e: DragEvent, activity: ActivityDescriptor) {
    const json = JSON.stringify(activity);
    const isTrigger = (activity.traits & ActivityTraits.Trigger) == ActivityTraits.Trigger;

    e.dataTransfer.setData('activity-descriptor', json);

    if (isTrigger) {
      e.dataTransfer.setData('trigger-descriptor', json);
    }
  }

  private static onTriggerStartDrag(e: DragEvent, trigger: TriggerDescriptor) {
    const json = JSON.stringify(trigger);
    const isActivity = false;

    e.dataTransfer.setData('trigger-descriptor', json);

    if (isActivity)
      e.dataTransfer.setData('activity-descriptor', json);
  }

  private onToggleActivityCategory(categoryModel: CategoryModel<ActivityDescriptor>) {
    categoryModel.expanded = !categoryModel.expanded;
    this.activityCategoryModels = [...this.activityCategoryModels];
  }

  private onToggleTriggerCategory(categoryModel: CategoryModel<TriggerDescriptor>) {
    categoryModel.expanded = !categoryModel.expanded;
    this.triggerCategoryModels = [...this.triggerCategoryModels];
  }

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

        {this.renderActivitiesTab()}
        {this.renderTriggersTab()}
      </div>
    );
  }

  private renderActivitiesTab() {
    const categoryModels = this.activityCategoryModels;
    const cssClass = this.selectedTabIndex == 0 ? '' : 'hidden';

    return <nav class={`${cssClass} flex-1 px-2 space-y-1 font-sans text-sm text-gray-600`}>
      {categoryModels.map(categoryModel => {

          const category = categoryModel.category;
          const activities = categoryModel.elements;
          const categoryButtonClass = categoryModel.expanded ? 'rotate-90' : '';
          const categoryContentClass = categoryModel.expanded ? '' : 'hidden';

          return <div class="space-y-1">
            <button type="button"
                    onClick={() => this.onToggleActivityCategory(categoryModel)}
                    class="text-gray-600 hover:bg-gray-50 hover:text-gray-900 group w-full flex items-center pr-2 py-2 text-left text-sm font-medium rounded-md focus:outline-none">
              <svg
                class={`${categoryButtonClass} text-gray-300 mr-2 flex-shrink-0 h-5 w-5 transform group-hover:text-gray-400 transition-colors ease-in-out duration-150`}
                viewBox="0 0 20 20" aria-hidden="true">
                <path d="M6 6L14 10L6 14V6Z" fill="currentColor"/>
              </svg>
              {category}
            </button>

            <div class={`space-y-1 ${categoryContentClass}`}>

              {activities.map(activity => (
                <div class="w-full flex items-center pl-10 pr-2 py-2">
                  <div class="cursor-move" onDragStart={e => ElsaActivityPicker.onActivityStartDrag(e, activity)}>
                    <ActivityDescriptorView activityDescriptor={activity}/>
                  </div>
                </div>
              ))}

            </div>
          </div>;
        }
      )}

    </nav>
  }

  private renderTriggersTab() {
    const categoryModels = this.triggerCategoryModels;
    const cssClass = this.selectedTabIndex == 1 ? '' : 'hidden';

    return <nav class={`${cssClass} flex-1 px-2 space-y-1 font-sans text-sm text-gray-600`}>
      {categoryModels.map(categoryModel => {

          const category = categoryModel.category;
          const triggers = categoryModel.elements;
          const categoryButtonClass = categoryModel.expanded ? 'rotate-90' : '';
          const categoryContentClass = categoryModel.expanded ? '' : 'hidden';

          return <div class="space-y-1">
            <button type="button"
                    onClick={() => this.onToggleTriggerCategory(categoryModel)}
                    class="text-gray-600 hover:bg-gray-50 hover:text-gray-900 group w-full flex items-center pr-2 py-2 text-left text-sm font-medium rounded-md focus:outline-none">
              <svg
                class={`${categoryButtonClass} text-gray-300 mr-2 flex-shrink-0 h-5 w-5 transform group-hover:text-gray-400 transition-colors ease-in-out duration-150`}
                viewBox="0 0 20 20" aria-hidden="true">
                <path d="M6 6L14 10L6 14V6Z" fill="currentColor"/>
              </svg>
              {category}
            </button>

            <div class={`space-y-1 ${categoryContentClass}`}>

              {triggers.map(trigger => (
                <div class="w-full flex items-center pl-10 pr-2 py-2">
                  <div class="cursor-move" onDragStart={e => ElsaActivityPicker.onTriggerStartDrag(e, trigger)}>
                    <TriggerDescriptorView triggerDescriptor={trigger}/>
                  </div>
                </div>
              ))}

            </div>
          </div>;
        }
      )}

    </nav>
  }
}

WorkflowEditorTunnel.injectProps(ElsaActivityPicker, ['activityDescriptors']);
