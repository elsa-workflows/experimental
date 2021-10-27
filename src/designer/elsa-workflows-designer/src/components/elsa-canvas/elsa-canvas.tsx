import {Component, h, Method, Element} from '@stencil/core';
import {Graph, Node} from '@antv/x6';
import './shapes';
import './connectors';
import './ports';
import {ActivityNode} from "./shapes";

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
        modifiers: ['ctrl', 'meta'],
      },
      scroller: {
        enabled: true,
        pannable: true,
        pageVisible: false,
        pageBreak: false,
        padding: 0,
      },
      connecting: {
        allowBlank: false,
        allowMulti: true,
        allowLoop: true,
        allowNode: true,
        allowEdge: false,
        allowPort: true,
        highlight: true,
        snap: {
          radius: 20,
        },
      },
      mousewheel: {
        enabled: true,
        modifiers: ['ctrl', 'meta'],
      },
      history: true,
      interacting: () => state.interactingMap,
    });

    graph.addNode({
      shape: 'activity',
      x: 80,
      y: 80,
      text: 'Delay'
    });

    const n = graph.addNode({
      shape: 'activity',
      x: 80,
      y: 180,
      width: 320,
      height: 80,
      text: 'HTTP Request',
      ports: [
        {
          id: 'port1',
          group: 'out',
        }
      ],
    }) as ActivityNode;

    setTimeout(() => {
      return n.text = 'A new label here';
    }, 3000);
  }

  render() {
    return (
      <div
        class="absolute left-0 top-0 right-0 bottom-0"
        ref={el => this.container = el}/>
    );
  }
}
