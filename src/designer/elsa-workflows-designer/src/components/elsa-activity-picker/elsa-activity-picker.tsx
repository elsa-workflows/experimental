import {Component, h, Event, EventEmitter, State, Prop, Watch} from "@stencil/core";
import {Addon, Graph} from '@antv/x6';

export interface ActivityPickerStateChangedArgs {
  expanded: boolean;
}

@Component({
  tag: 'elsa-activity-picker',
  styleUrl: 'elsa-activity-picker.scss',
})
export class ElsaActivityPicker {
  @Prop() graph: Graph;
  @Event() expandedStateChanged: EventEmitter<ActivityPickerStateChangedArgs>;
  @State() isExpanded: boolean = true;
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

  startDrag = (e: Event) => {
    const target = e.currentTarget

    const node =
      this.graph.createNode({
        shape: 'activity',
        text: 'HTTP Response',
        ports: [
          {
            id: 'port1',
            group: 'out',
            attrs: {
              text: {
                text: 'next',
              },
            }
          }
        ]
      });

    this.dnd.start(node, e as MouseEvent);
  }

  private onToggleClick = () => {
    this.isExpanded = !this.isExpanded;
    this.expandedStateChanged.emit({expanded: this.isExpanded});
  };

  render() {
    return (
      <div class="activity-picker absolute left-0 top-0 bottom-0 transition-all duration-200 ease-in-out border-r">

        <nav class="activity-list flex-1 px-2 space-y-1 font-sans text-sm text-gray-600">
          <div class="space-y-1">
            <button type="button"
                    class="text-gray-600 hover:bg-gray-50 hover:text-gray-900 group w-full flex items-center pr-2 py-2 text-left text-sm font-medium rounded-md focus:outline-none">
              <svg
                class="text-gray-300 mr-2 flex-shrink-0 h-5 w-5 transform group-hover:text-gray-400 transition-colors ease-in-out duration-150"
                viewBox="0 0 20 20" aria-hidden="true">
                <path d="M6 6L14 10L6 14V6Z" fill="currentColor"/>
              </svg>
              Primitives
            </button>

            <div class="space-y-1"
                 id="sub-menu-1" style={{display: "block"}}>

              {[...Array(5)].map(x => (
                <div class="w-full flex items-center pl-10 pr-2 py-2">
                  <elsa-activity class="cursor-move"
                                 onMouseDown={e => this.startDrag(e)}/>
                </div>
              ))}

            </div>
          </div>


        </nav>

        <div class="activity-picker-toggle panel-toggle text-white" onClick={() => this.onToggleClick()}>
          <svg class="h-6 w-6 text-gray-700" width="24" height="24" viewBox="0 0 24 24" stroke-width="2"
               stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
            <path stroke="none" d="M0 0h24v24H0z"/>
            <polyline points="9 6 15 12 9 18"/>
          </svg>
        </div>
      </div>
    );
  }
}
