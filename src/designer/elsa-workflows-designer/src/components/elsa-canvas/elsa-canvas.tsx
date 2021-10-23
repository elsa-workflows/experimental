import {Component, h} from '@stencil/core';
import {Graph, Node, Shape} from '@antv/x6';

@Component({
  tag: 'elsa-canvas',
  styleUrl: 'elsa-canvas.scss',
  shadow: false,
  scoped: true
})
export class ElsaCanvas {

  private containerElement: HTMLElement;
  private target: Node;

  componentDidLoad() {

    const data = {
      // 节点
      nodes: [
        {
          id: 'node1', // String，可选，节点的唯一标识
          x: 40,       // Number，必选，节点位置的 x 值
          y: 40,       // Number，必选，节点位置的 y 值
          width: 80,   // Number，可选，节点大小的 width 值
          height: 40,  // Number，可选，节点大小的 height 值
          label: 'hello', // String，节点标签
          shape: 'rect',
          "attrs": {
            "body": {
              "rx": 10,
              "ry": 10
            }
          },
        },
        {
          id: 'node2', // String，节点的唯一标识
          x: 160,      // Number，必选，节点位置的 x 值
          y: 180,      // Number，必选，节点位置的 y 值
          width: 80,   // Number，可选，节点大小的 width 值
          height: 40,  // Number，可选，节点大小的 height 值
          label: 'world', // String，节点标签
        },
      ],
      // 边
      edges: [
        {
          source: 'node1', // String
          target: 'node2', // String
        },
      ],
    };

    // Create an instance of Graph.
    const graph = new Graph({
      container: this.containerElement,
      panning: {
        enabled: true,
        modifiers: 'shift',
      },
      allowRubberband: x => true,
      autoResize: true,
      async: true,
      selecting: {
        enabled: true,
      },
      background: {
        color: '#fffbe6', // 设置画布背景颜色
      },
      grid: {
        size: 10,      // 网格大小 10px
        visible: true, // 渲染网格背景
      },
    });

    graph.fromJSON(data);
  }

  render() {
    return <div>
      <div id="container"
           style={{width: '100%', height: '400px'}}
           ref={el => this.containerElement = el}/>
      <button class="btn" onClick={e => this.onClick(e)}>Click Me</button>
    </div>;
  }

  private onClick = (e: Event) => {
    //const p = this.target.getPosition();
    //p.x += 50;
    //this.target.setPosition(p);
  };
}
