import {Dom, Graph, Shape} from "@antv/x6";
import {Rect} from "@antv/x6/lib/shape/standard";

const ports = {
  groups: {
    top: {
      position: 'top',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff',
        },
      },
    },
    right: {
      position: 'right',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff',
        },
      },
    },
    bottom: {
      position: 'bottom',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff'
        },
      },
    },
    left: {
      position: 'left',
      attrs: {
        circle: {
          r: 4,
          magnet: true,
          stroke: '#5F95FF',
          strokeWidth: 1,
          fill: '#fff'
        },
      },
    },
  },
  items: [
    {
      group: 'top',
    },
    {
      group: 'right',
    },
    {
      group: 'bottom',
    },
    {
      group: 'left',
    },
  ],
}

Graph.registerNode(
  'activity',
  {
    inherit: 'rect',
    width: 130,
    height: 36,
    attrs: {
      body: {
        strokeWidth: 1,
        stroke: '#108ee9',
        fill: '#fff',
        rx: 10,
        ry: 10,
      },
      text: {
        fontSize: 12,
        color: 'red',
      },
    },
    ports: {...ports}
  },
  true,
)

// Graph.registerNode(
//   'activity-2',
//   {
//     shape: 'activity-2',
//     html(){
//       const wrap = document.createElement('div');
//
//       wrap.innerHTML = `
//       <div>
//           TEST
//         </div>
//       `;
//
//       return wrap;
//     }
//   });
