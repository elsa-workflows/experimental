import {Graph} from "@antv/x6";

Graph.registerPortLayout('dynamic', (portsPositionArgs, elemBBox) => {
  return portsPositionArgs.map((_, index) => {

    return {
      position: {
        x: index,
        y: elemBBox.height,
      },
      angle: 0,
    }
  })
})
