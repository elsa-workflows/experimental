import {Graph, Node} from "@antv/x6";
import {Activity, ActivityDescriptor} from "../../../models";

export function createNode(graph: Graph, activityDescriptor: ActivityDescriptor, activity: Activity, x: number, y: number): Node<Node.Properties> {
  return graph.createNode({
    shape: 'activity',
    text: activityDescriptor.displayName,
    x: x,
    y: y,
    data: activity,
    ports: [
      {
        id: 'inbound1',
        group: 'in',
        attrs: {
          text: {
            text: 'In'
          }
        }
      },
      {
        id: 'outbound1',
        group: 'out',
        attrs: {
          text: {
            text: 'Done'
          }
        }
      }
    ]
  });
}
