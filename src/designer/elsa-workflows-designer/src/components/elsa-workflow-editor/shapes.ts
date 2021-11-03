import {Cell, Dom, Graph, Shape} from "@antv/x6";
import {Rect} from "@antv/x6/lib/shape/standard";
import {ForceDirected} from "@antv/x6/lib/layout/force-directed";
import NodeData = ForceDirected.NodeData;
import {HTML} from "@antv/x6/es/shape/standard";
import {PortManager} from "@antv/x6/lib/model/port";
import {merge} from "jquery";
import {config} from "../../../stencil.config";

const ports = {
  groups: {
    top: {
      position: 'top',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff',
        },
      },
    },
    right: {
      position: 'right',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff',
        },
      },
    },
    bottom: {
      position: 'bottom',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff'
        },
      },
    },
    left: {
      position: 'left',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff'
        },
      },
    },
  },
  items: [
    {
      group: 'top',
    },
    {
      group: 'right',
    },
    {
      group: 'bottom',
    },
    {
      group: 'left',
    },
  ],
}

Graph.registerNode(
  'activity-old',
  {
    inherit: 'rect',
    width: 130,
    height: 36,
    attrs: {
      body: {
        strokeWidth: 1,
        stroke: '#108ee9',
        fill: '#fff',
        rx: 10,
        ry: 10,
      },
      text: {
        fontSize: 12,
        color: 'red',
      },
    },
    ports: {...ports}
  },
  true,
)

export class ActivityNode extends Shape.HTML {
  get text() {
    return this.store.get<string>('text');
  }

  set text(value: string) {
    this.store.set('text', value);
  }

  init() {
    super.init();
    this.updateSize();
  }

  setup() {
    const self = this;
    super.setup();
    this.on('change:text', this.updateSize, this);

    this.html = {
      render() {

        return self.createHtml();

      },
      shouldComponentUpdate(node: Cell) {
        return node.hasChanged('text');
      },
    };
  }

  updateSize() {
    const wrapper = document.createElement('div');
    wrapper.className = 'w-full flex items-center pl-10 pr-2 py-2';
    wrapper.innerHTML = this.createHtml();
    document.body.append(wrapper);
    const rect = wrapper.firstElementChild.getBoundingClientRect();
    console.debug(rect);
    wrapper.remove();

    const width = rect.width;
    const height = rect.height;
    this.prop({size: {width, height}});
  }

  createHtml() {
    const text = this.text;
    return (`
          <div>
            <div class="activity-wrapper border border-solid border-blue-600 rounded bg-blue-400 text-white overflow-hidden">
              <div class="flex flex-row">
                <div class="flex flex-shrink items-center bg-blue-500">
                  <div class="px-2 py-1">
                    <svg class="h-6 w-6 text-white" width="24" height="24" viewBox="0 0 24 24"
                         stroke-width="2"
                         stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                      <path stroke="none" d="M0 0h24v24H0z"/>
                      <path d="M7 18a4.6 4.4 0 0 1 0 -9h0a5 4.5 0 0 1 11 2h1a3.5 3.5 0 0 1 0 7h-12"/>
                    </svg>
                  </div>
                </div>
                <div class="flex items-center">
                  <div class="px-4 py-2">
                    ${text}
                  </div>
                </div>
              </div>
            </div>
          </div>
        `);
  }
}

ActivityNode.config({
  ports: {
    groups: {
      in: {
        position: 'dynamicIn',
        attrs: {
          circle: {
            r: 6,
            magnet: true,
            stroke: '#3c82f6',
            strokeWidth: 2,
            fill: '#fff',
          },
        },
      },
      out: {
        position: 'dynamicOut',
        attrs: {
          circle: {
            r: 6,
            magnet: true,
            stroke: '#fff',
            strokeWidth: 2,
            fill: '#3c82f6',
          },
        },
      },
    },
  },
});

Graph.registerNode('activity', ActivityNode, true);
