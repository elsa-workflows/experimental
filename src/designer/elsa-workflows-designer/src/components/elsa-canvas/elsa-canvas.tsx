import {Component, h, Method, Element} from '@stencil/core';
import {Graph, Node, Shape} from '@antv/x6';
import './shapes';
import './connectors';
import './ports';

@Component({
  tag: 'elsa-canvas',
  styleUrl: 'elsa-canvas.scss',
})
export class ElsaCanvas {

  @Element() el: HTMLElement;
  private container: HTMLElement;
  private graph: Graph;
  private target: Node;

  @Method()
  public async getGraph(): Promise<Graph> {
    return this.graph;
  };

  @Method()
  public async resize(width?: number, height?: number): Promise<void> {
    this.graph.resize(width, height);
  }

  @Method()
  public async updateLayout(): Promise<void> {
    const width = this.el.clientWidth;
    const height = this.el.clientHeight;
    this.graph.resize(width, height);
  }

  componentDidLoad() {

    const state = {
      interactingMap: {
        magnetConnectable: true,
        edgeMovable: true,
        edgeLabelMovable: true,
        arrowheadMovable: true,
        vertexMovable: true,
        vertexAddable: true,
        vertexDeletable: true,
      },
    }

    const graph = this.graph = new Graph({
      container: this.container,
      grid: true,
      async: true,
      autoResize: true,
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
        connectionPoint: 'anchor',
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
      mousewheel: {
        enabled: true,
        modifiers: ['ctrl', 'meta'],
      },
      history: true,
      interacting: () => state.interactingMap,
    });
  }

  render() {
    return (
      <div
        class="absolute left-0 top-0 right-0 bottom-0"
        ref={el => this.container = el}/>
    );
  }
}
