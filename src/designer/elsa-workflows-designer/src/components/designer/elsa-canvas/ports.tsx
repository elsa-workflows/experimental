import {Graph, Line} from "@antv/x6";
import {toResult} from "@antv/x6/lib/registry/port-layout/util";

Graph.registerPortLayout('dynamicOut', (portsPositionArgs, elemBBox) => {
  return portsPositionArgs.map((_, index) => {

    const portCount = portsPositionArgs.length;
    const ratio = (index + 0.5) / portCount;
    const p1 = portCount <= 3 ? elemBBox.getTopRight() : elemBBox.getBottomLeft();
    const p2 = portCount <= 3 ? elemBBox.getBottomRight() : elemBBox.getBottomRight();
    const line = new Line(p1, p2)
    const p = line.pointAt(ratio);
    
    return toResult(p.round(), 0, {});

  });
});

Graph.registerPortLayout('dynamicIn', (portsPositionArgs, elemBBox) => {
  return portsPositionArgs.map((_, index) => {

    const portCount = portsPositionArgs.length;
    const ratio = (index + 0.5) / portCount;
    const p1 = portCount <= 3 ? elemBBox.getTopLeft() : elemBBox.getTopLeft();
    const p2 = portCount <= 3 ? elemBBox.getBottomLeft() : elemBBox.getTopRight();
    const line = new Line(p1, p2)
    const p = line.pointAt(ratio);

    return toResult(p.round(), 0, {});

  });
});
