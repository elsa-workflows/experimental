import {Component, h, Method} from '@stencil/core';
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
      width: 1024,
      height: 760,
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
        pageVisible: true,
        pageBreak: false,
        padding: 20,
        //minVisibleWidth: 2048,
        //minVisibleHeight: 2048
      },
      connecting:{
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
    return <div>
      <div id="container"
           style={{width: '100%', height: '600px'}}
           ref={el => this.container = el}/>
      <button class="btn" onClick={e => this.onClick(e)}>Click Me</button>
    </div>;
  }

  private onClick = (e: Event) => {
    //const p = this.target.getPosition();
    //p.x += 50;
    //this.target.setPosition(p);
  };
}
