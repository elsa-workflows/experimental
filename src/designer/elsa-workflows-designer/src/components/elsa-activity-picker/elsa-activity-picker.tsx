import {Component, h, Event, EventEmitter, State, Prop, Watch} from "@stencil/core";
import {Addon, Graph} from '@antv/x6';
import groupBy from 'lodash/groupBy';
import {ActivityDescriptor} from "../../models";
import {Activity} from "../elsa-activity/elsa-activity";

@Component({
  tag: 'elsa-activity-picker',
  styleUrl: 'elsa-activity-picker.scss',
})
export class ElsaActivityPicker {
  @Prop() graph: Graph;
  @State() activityDescriptors: Array<ActivityDescriptor> = [];
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

  componentWillLoad() {
    const cats = {
      primitives: 'Primitives',
      console: 'Console',
      http: 'HTTP'
    };

    this.activityDescriptors = [
      {
        activityType: 'Assign',
        displayName: 'Assign',
        category: cats.primitives
      },
      {
        activityType: 'WriteLine',
        displayName: 'Write Line',
        category: cats.console
      },
      {
        activityType: 'ReadLine',
        displayName: 'Read Line',
        category: cats.console
      },
      {
        activityType: 'SendHttpRequest',
        displayName: 'Send HTTP Request',
        category: cats.http
      },
      {
        activityType: 'HttpTrigger',
        displayName: 'HTTP Trigger',
        category: cats.http
      },
      {
        activityType: 'WriteHttpResponse',
        displayName: 'Write HTTP Response',
        category: cats.http
      }
    ];
  }

  startDrag = (e: Event, activityDescriptor: ActivityDescriptor) => {
    const target = e.currentTarget

    const node =
      this.graph.createNode({
        shape: 'activity',
        text: activityDescriptor.displayName,
        ports: [
          {
            id: 'inbound1',
            group: 'in',
          },
          {
            id: 'outbound1',
            group: 'out',
          }
        ]
      });

    this.dnd.start(node, e as MouseEvent);
  }

  render() {
    const activityDescriptors = this.activityDescriptors;
    const categorizedActivitiesLookup = groupBy(activityDescriptors, x => x.category);
    const categories = Object.keys(categorizedActivitiesLookup);

    return (

      <nav class="activity-list flex-1 px-2 space-y-1 font-sans text-sm text-gray-600">
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
                    <div class="cursor-move" onMouseDown={e => this.startDrag(e, activity)}>
                      <Activity activityDescriptor={activity}/>
                    </div>
                  </div>
                ))}

              </div>
            </div>;
          }
        )}

      </nav>
    );
  }
}
