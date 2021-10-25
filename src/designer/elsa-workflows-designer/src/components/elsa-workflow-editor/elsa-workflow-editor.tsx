import {Component, h} from "@stencil/core";
import { Shape, Addon } from '@antv/x6';

@Component({
  tag: 'elsa-workflow-editor',
  styleUrl: 'elsa-workflow-editor.scss',
})
export class ElsaWorkflowEditor {

  private canvas: HTMLElsaCanvasElement;
  private container: HTMLDivElement;

  async componentDidLoad(){
    const graph = await this.canvas.getGraph();

    const stencil = new Addon.Stencil({
      title: 'Activities',
      target: graph,
      stencilGraphWidth: 400,
      stencilGraphHeight: 180,
      collapsable: false,
      groups: [
        {
          title: 'HTTP',
          name: 'http',
        },
        {
          title: 'Primitives',
          name: 'primitives',
          //graphHeight: 250,
          layoutOptions: {
            resizeToFit: true
          },
        },
      ],
      layoutOptions: {
        columns: 2,
        columnWidth: 170,
        rowHeight: 60,
        resizeToFit: true
      },
    });

    const activities = [...Array(10)].map((x, i) => graph.createNode({
      shape: 'activity',
      label: `activity ${i}`,
    }));

    stencil.load(activities, 'http');
    stencil.load(activities, 'primitives');

    this.container.appendChild(stencil.container);
  }

  render() {
    return (
      <div class="flex">
        <div class="w-96 relative" ref={el => this.container = el}>Stencil</div>
        <div class="flex-auto">
          <elsa-canvas ref={el => this.canvas = el}/>
        </div>
      </div>
    );
  }
}
