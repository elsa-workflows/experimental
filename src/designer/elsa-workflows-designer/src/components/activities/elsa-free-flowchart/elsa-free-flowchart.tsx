import {Component, Element, Event, EventEmitter, h, Method} from '@stencil/core';
import {Edge, Graph, Node, NodeView} from '@antv/x6';
import {v4 as uuid} from 'uuid';
import _, {first} from 'lodash';
import '../../../models/shapes';
import '../../../models/ports';
import {ActivityComponent} from "../activity-component";
import {AddActivityArgs} from "../../designer/elsa-canvas/elsa-canvas";
import {Activity, ActivityEditRequestArgs, ActivityInput, GraphUpdatedArgs} from "../../../models";
import {createGraph} from "./graph-factory";
import {createNode} from "./node-factory";
import {Connection, Flowchart} from "./models";
import PositionEventArgs = NodeView.PositionEventArgs;

@Component({
  tag: 'elsa-free-flowchart',
  styleUrl: 'elsa-free-flowchart.scss',
})
export class ElsaFreeFlowchart implements ActivityComponent {

  @Element() el: HTMLElement;
  private container: HTMLElement;
  private graph: Graph;
  private target: Node;

  @Event() activityEditRequested: EventEmitter<ActivityEditRequestArgs>;
  @Event() graphUpdated: EventEmitter<GraphUpdatedArgs>;

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
      metadata: {},
    };

    const node = createNode(graph, descriptor, activity, x, y);
    graph.addNode(node);

    const json = graph.toJSON();
  }

  public async componentDidLoad() {
    const graph = this.graph = createGraph(this.container);

    graph.on('node:click', this.onNodeClick);
    graph.on('edge:connected', this.onEdgeConnected);

    graph.on('node:change:*', this.onGraphChanged);
    graph.on('node:added', this.onGraphChanged);
    graph.on('node:removed', this.onGraphChanged);
    graph.on('edge:added', this.onGraphChanged);
    graph.on('edge:removed', this.onGraphChanged);
    graph.on('edge:connected', this.onGraphChanged);

    await this.updateLayout();
  }

  exportGraphInternal = (): Activity => {
    const graph = this.graph;
    const graphModel = graph.toJSON();
    const activities = graphModel.cells.filter(x => x.shape == 'activity').map(x => x.data as Activity);
    const connections = graphModel.cells.filter(x => x.shape == 'edge' && !!x.data).map(x => x.data as Connection);

    const flowchart: Flowchart = {
      activityType: 'Workflows.Flowchart',
      metadata: {},
      activities: activities,
      connections: connections,
      id: "1",
      start: _.first(activities)?.id,
      variables: []
    }

    return flowchart;
  }

  onNodeClick = async (e: PositionEventArgs<JQuery.ClickEvent>) => {
    const node = e.node;
    const activity = node.data as Activity;

    const args: ActivityEditRequestArgs = {
      activity: activity,
      applyChanges: a => node.data = a,
      deleteActivity: a => node.remove({deep: true})
    };

    this.activityEditRequested.emit(args);
  };

  onEdgeConnected = (e: { isNew: boolean, edge: Edge }) => {
    const edge = e.edge;
    const isNew = e.isNew;
    const targetNode = edge.getTargetNode();
    const sourceActivity = edge.getSourceNode().data as Activity;
    const targetActivity = targetNode.data as Activity;
    const outboundPort = targetNode.getPort(edge.getTargetPortId()).id;

    edge.data = {
      source: sourceActivity.id,
      target: targetActivity.id,
      outboundPort: outboundPort
    };
  }

  onGraphChanged = async (e: any) => {
    console.debug('changed');

    this.graphUpdated.emit({exportGraph: this.exportGraphInternal});
  }

  render() {
    return (
      <div
        class="absolute left-0 top-0 right-0 bottom-0"
        ref={el => this.container = el}/>
    );
  }
}
