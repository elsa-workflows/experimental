import {Activity, ActivityDescriptor, Container} from "../../../models";

export interface ActivityNode {
  activity: Activity;
  parents: Array<ActivityNode>;
  children: Array<ActivityNode>;
  port?: string;
}

export interface ActivityPort {
  activity: Activity;
  port: string;
}

export function walkActivities(root: Activity, descriptors: Array<ActivityDescriptor>): ActivityNode {
  const collectedActivities = new Set<Activity>([root]);
  const graph: ActivityNode = {activity: root, parents: [], children: []};
  const collectedNodes = new Set<ActivityNode>([graph]);
  walkRecursive(graph, root, collectedActivities, collectedNodes, descriptors);
  return graph;
}

export function flatten(root: ActivityNode): Array<ActivityNode> {
  const list: Array<ActivityNode> = [root];

  for (const node of root.children) {
    const children = flatten(node);

    for (const child of children)
      list.push(child);
  }

  return list;
}

function walkRecursive(node: ActivityNode, activity: Activity, collectedActivities: Set<Activity>, collectedNodes: Set<ActivityNode>, descriptors: Array<ActivityDescriptor>) {
  const ports = getPorts(node, activity, descriptors);

  for (const port of ports) {
    const collectedNodesArray = Array.from(collectedNodes);
    let childNode = collectedNodesArray.find(x => x.activity == port.activity);

    if (!childNode) {
      childNode = {activity: activity, children: [], parents: [], port: port.port};
      collectedNodes.add(childNode);
    }

    childNode.parents.push(node);
    node.children.push(childNode);
    collectedActivities.add(port.activity);
    walkRecursive(childNode, port.activity, collectedActivities, collectedNodes, descriptors);
  }
}

function getPorts(node: ActivityNode, activity: Activity, descriptors: Array<ActivityDescriptor>): Array<ActivityPort> {
  const descriptor = descriptors.find(x => x.activityType == activity.activityType);

  if (!descriptor)
    return [];

  let ports: Array<ActivityPort> = [];

  for (const outPort of descriptor.outPorts) {
    const outbound: Activity | Array<Activity> = activity[outPort.name];

    if (!!outbound) {
      if (Array.isArray(outbound)) {
        for (const a of outbound)
          ports.push({activity: a, port: outPort.name});
      } else
        ports.push({activity: outbound, port: outPort.name});
    }
  }

  return ports;
}
