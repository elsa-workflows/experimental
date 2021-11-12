import {Component, Element, Event, EventEmitter, h, Host, Method} from '@stencil/core';
import {Graph, Node, Shape} from '@antv/x6';
import {v4 as uuid} from 'uuid';
import '../../../models/shapes';
import '../../../models/ports';
import {ActivityComponent} from "../activity-component";
import {AddActivityArgs} from "../../designer/elsa-canvas/elsa-canvas";
import {Activity} from "../../../models";
import {createGraph} from "./graph-factory";
import {createNode} from "./node-factory";

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
    const width = this.el.clientWidth;
    const height = this.el.clientHeight;
    this.graph.resize(width, height);
    this.graph.updateBackground();
  }

  @Method()
  public async addActivity(args: AddActivityArgs): Promise<void> {
    const graph = this.graph;
    const {descriptor, x, y} = args;

    const activity: Activity = {
      id: uuid(),
      activityType: descriptor.activityType,
      metadata: {}
    };

    const node = createNode(graph, descriptor, activity, x, y);
    graph.addNode(node);

    const json = graph.toJSON();
    console.debug(json);
  }

  async componentDidLoad() {
    const graph = this.graph = createGraph(this.container);

    graph.on('node:dblclick', this.onNodeDoubleClick);

    await this.updateLayout();
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
