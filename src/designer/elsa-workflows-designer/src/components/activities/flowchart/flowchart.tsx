import {Component, Element, Event, EventEmitter, h, Method, Prop} from '@stencil/core';
import {Edge, Graph, Node, NodeView} from '@antv/x6';
import {v4 as uuid} from 'uuid';
import {camelCase, first} from 'lodash';
import './shapes';
import './ports';
import {ContainerActivityComponent} from '../container-activity-component';
import {AddActivityArgs} from '../../designer/canvas/canvas';
import {
  Activity,
  ActivityDescriptor,
  ActivitySelectedArgs,
  ContainerSelectedArgs,
  GraphUpdatedArgs
} from '../../../models';
import {createGraph} from './graph-factory';
import {createNode} from './node-factory';
import {Connection, Flowchart} from './models';
import PositionEventArgs = NodeView.PositionEventArgs;
import WorkflowEditorTunnel from '../../designer/state';

@Component({
  tag: 'elsa-flowchart',
  styleUrl: 'flowchart.scss',
})
export class FlowchartComponent implements ContainerActivityComponent {

  @Prop({mutable: true}) public activityDescriptors: Array<ActivityDescriptor> = [];

  @Element() el: HTMLElement;
  container: HTMLElement;
  graph: Graph;
  target: Node;

  @Event() activitySelected: EventEmitter<ActivitySelectedArgs>;
  @Event() containerSelected: EventEmitter<ContainerSelectedArgs>;
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

  @Method()
  public async exportGraph(): Promise<Activity> {
    return this.exportGraphInternal();
  }

  public async componentDidLoad() {
    const graph = this.graph = createGraph(this.container);

    graph.on('blank:click', this.onGraphClick);
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
    const connections = graphModel.cells.filter(x => x.shape == 'elsa-edge' && !!x.data).map(x => x.data as Connection);
    const remainingConnections: Array<Connection> = []; // The connections remaining after transposition.
    let remainingActivities: Array<Activity> = [...activities]; // The activities remaining after transposition.
    const activityDescriptors = this.activityDescriptors;

    // Transpose connections to activity outbound properties where applicable.

    for (const connection of connections) {
      const source = activities.find(x => x.id == connection.source);
      const target = activities.find(x => x.id == connection.target);
      const sourceDescriptor = activityDescriptors.find(x => x.activityType == source.activityType);
      const targetDescriptor = activityDescriptors.find(x => x.activityType == target.activityType);
      const matchingTargetPort = sourceDescriptor.outPorts.find(x => x.name == connection.sourcePort);

      if (!!matchingTargetPort) {
        // Assign the target activity directly to the out port of the source activity.
        const outPortPropName = camelCase(connection.sourcePort);
        source[outPortPropName] = target;

        // Remove the target activity from the list.
        remainingActivities = remainingActivities.filter(x => x != target);
      } else {
        // Keep this connection.
        remainingConnections.push(connection);
      }
    }

    return {
      activityType: 'Workflows.Flowchart',
      metadata: {},
      activities: remainingActivities,
      connections: remainingConnections,
      id: "1",
      start: first(activities)?.id,
      variables: []
    };
  }

  onGraphClick = async (e: PositionEventArgs<JQuery.ClickEvent>) => this.containerSelected.emit({});

  onNodeClick = async (e: PositionEventArgs<JQuery.ClickEvent>) => {
    const node = e.node;
    const activity = node.data as Activity;

    const args: ActivitySelectedArgs = {
      activity: activity,
      applyChanges: a => node.data = a,
      deleteActivity: a => node.remove({deep: true})
    };

    this.activitySelected.emit(args);
  };

  onEdgeConnected = (e: { isNew: boolean, edge: Edge }) => {
    const edge = e.edge;
    const isNew = e.isNew;
    const sourceNode = edge.getSourceNode();
    const targetNode = edge.getTargetNode();
    const sourceActivity = edge.getSourceNode().data as Activity;
    const targetActivity = targetNode.data as Activity;
    const sourcePort = sourceNode.getPort(edge.getSourcePortId()).id;
    const targetPort = targetNode.getPort(edge.getTargetPortId()).id;

    // noinspection UnnecessaryLocalVariableJS
    const connection: Connection = {
      source: sourceActivity.id,
      sourcePort: sourcePort,
      target: targetActivity.id,
      targetPort: targetPort
    };

    edge.data = connection;
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

WorkflowEditorTunnel.injectProps(FlowchartComponent, ['activityDescriptors']);
