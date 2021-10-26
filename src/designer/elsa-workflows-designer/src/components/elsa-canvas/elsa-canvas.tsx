import {Component, h, Host, Method} from '@stencil/core';
import {CellView, Dom, Graph, Node, Platform, Shape} from '@antv/x6';
import './shapes';
import './connectors';
import './ports';

@Component({
  tag: 'elsa-canvas',
  styleUrl: 'elsa-canvas.scss',
})
export class ElsaCanvas {

  private container: HTMLElement;
  private graph: Graph;
  private target: Node;

  @Method()
  async getGraph(): Promise<Graph> {
    return this.graph;
  };

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
          radius: 50,
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
      label: 'http',
    });

    graph.addNode({
      shape: 'activity',
      x: 80,
      y: 180,
      label: 'http response',
    });

  }

  render() {
    return (
      <Host id="container"
           class="absolute left-0 top-0 right-0 bottom-0"
           ref={el => this.container = el}/>
    );
  }
}
