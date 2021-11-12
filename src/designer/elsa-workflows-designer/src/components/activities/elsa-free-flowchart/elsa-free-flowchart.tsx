import {Component, Element, Event, EventEmitter, h, Host, Method} from '@stencil/core';
import {Graph, Node, Shape} from '@antv/x6';
import '../../../models/shapes';
import '../../../models/ports';
import {ActivityComponent} from "../activity-component";
import {AddActivityArgs} from "../../designer/elsa-canvas/elsa-canvas";

@Component({
  tag: 'elsa-free-flowchart',
  styleUrl: 'elsa-free-flowchart.scss',
})
export class ElsaFreeFlowchart implements ActivityComponent {

  @Element() el: HTMLElement;
  private container: HTMLElement;
  private graph: Graph;
  private target: Node;

  @Event() activityEditorRequested: EventEmitter;

  @Method()
  public async updateLayout(): Promise<void> {
    console.debug('update from free flowchart');
    const width = this.el.clientWidth;
    const height = this.el.clientHeight;
    console.debug({width, height});
    this.graph.resize(width, height);
  }

  @Method()
  public async addActivity(args: AddActivityArgs): Promise<void> {
    const graph = this.graph;
    const {descriptor, x, y} = args;

    const node =
      graph.createNode({
        shape: 'activity',
        text: descriptor.displayName,
        x: x,
        y: y,
        ports: [
          {
            id: 'inbound1',
            group: 'in',
            attrs: {
              text: {
                text: 'In'
              }
            }
          },
          {
            id: 'outbound1',
            group: 'out',
            attrs: {
              text: {
                text: 'Done'
              }
            }
          }
        ]
      });

    graph.addNode(node);

  }

  componentDidLoad() {

    const graph = this.graph = new Graph({
      container: this.container,
      grid: {
        type: 'mesh',
        size: 20,
        visible: true,
        args: {
          color: '#e0e0e0'
        }
      },
      async: true,
      autoResize: true,
      keyboard: {
        enabled: true,
        global: false,
      },
      clipboard: {
        enabled: true,
        useLocalStorage: true,
      },
      selecting: {
        enabled: true,
        showNodeSelectionBox: true,
        rubberband: true,
      },
      scroller: {
        enabled: true,
        pannable: true,
        pageVisible: false,
        pageBreak: false,
        padding: 0,
        modifiers: ['ctrl', 'meta'],
      },
      connecting: {
        allowBlank: false,
        allowMulti: true,
        allowLoop: true,
        allowNode: true,
        allowEdge: false,
        allowPort: true,
        highlight: true,
        router: 'manhattan',
        connector: {
          name: 'rounded',
          args: {
            radius: 20
          },
        },
        snap: {
          radius: 20,
        },
        validateMagnet({magnet}) {
          return magnet.getAttribute('port-group') !== 'in'
        },
        validateConnection({sourceView, targetView, sourceMagnet, targetMagnet}) {
          if (!sourceMagnet || sourceMagnet.getAttribute('port-group') === 'in') {
            return false
          }

          if (!targetMagnet || targetMagnet.getAttribute('port-group') !== 'in') {
            return false
          }

          const portId = targetMagnet.getAttribute('port')!
          const node = targetView.cell as Node
          const port = node.getPort(portId)
          return !(port && port.connected);


        },
        createEdge() {
          return new Shape.Edge({
            attrs: {
              line: {
                stroke: '#7b7b7b',
              },
            },
          })
        }
      },
      onPortRendered(args) {
        const selectors = args.contentSelectors
        const container = selectors && selectors.foContent
        if (container) {
          const port = document.createElement('div');
          port.className = 'rounded-full border border-2 border-blue h-8 w-8';
          port.innerHTML = `<p>done</p>`;
          (container as HTMLElement).append(port);
        }
      },
      mousewheel: {
        enabled: true,
        modifiers: ['ctrl', 'meta'],
      },
      history: {
        enabled: true,
        beforeAddCommand: (e: string, args: any) => {

          if (args.key == 'tools')
            return false;

          const supportedEvents = ['cell:added', 'cell:removed', 'cell:change:*'];
          console.debug({e, args});

          return supportedEvents.indexOf(e) >= 0;
        },
      },
      // Todo:
      // minimap: {
      //   enabled: true,
      //   container: this.container,
      // },
      //interacting: () => state.interactingMap,
    });

    graph.on('edge:mouseenter', ({edge}) => {
      edge.addTools([
        'source-arrowhead',
        'target-arrowhead',
        {
          name: 'button-remove',
          args: {
            distance: -30,
          },
        },
      ])
    });

    graph.on('edge:mouseleave', ({edge}) => {
      edge.removeTools()
    });

    graph.on('node:dblclick', this.onNodeDoubleClick);

    graph.bindKey(['meta+c', 'ctrl+c'], () => {
      const cells = graph.getSelectedCells()
      if (cells.length) {
        graph.copy(cells)
      }
      return false
    });

    graph.bindKey(['meta+x', 'ctrl+x'], () => {
      const cells = graph.getSelectedCells()
      if (cells.length) {
        graph.cut(cells)
      }
      return false
    });

    graph.bindKey(['meta+v', 'ctrl+v'], () => {
      if (!graph.isClipboardEmpty()) {
        const cells = graph.paste({offset: 32})
        graph.cleanSelection()
        graph.select(cells)
      }
      return false
    });

    //undo redo
    graph.bindKey(['meta+z', 'ctrl+z'], () => {
      if (graph.history.canUndo()) {
        graph.history.undo()
      }
      return false
    });

    graph.bindKey(['meta+y', 'ctrl+y'], () => {
      if (graph.history.canRedo()) {
        graph.history.redo()
      }
      return false
    });

    // select all;
    graph.bindKey(['meta+a', 'ctrl+a'], () => {
      const nodes = graph.getNodes()
      if (nodes) {
        graph.select(nodes)
      }
    });

    //delete
    graph.bindKey('del', () => {
      const cells = graph.getSelectedCells()
      if (cells.length) {
        graph.removeCells(cells)
      }
    });

    // zoom
    graph.bindKey(['ctrl+1', 'meta+1'], () => {
      const zoom = graph.zoom()
      if (zoom < 1.5) {
        graph.zoom(0.1)
      }
    });

    graph.bindKey(['ctrl+2', 'meta+2'], () => {
      const zoom = graph.zoom()
      if (zoom > 0.5) {
        graph.zoom(-0.1)
      }
    });
  }

  private onNodeDoubleClick = async () => {
    this.activityEditorRequested.emit();
  };

  render() {
    return (
      <div
        class="absolute left-0 top-0 right-0 bottom-0"
        ref={el => this.container = el}/>
    );
  }
}
