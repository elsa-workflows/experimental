import {Component, Listen, h} from "@stencil/core";
import {PanelOrientation, PanelStateChangedArgs} from "../elsa-panel/models";
import {ActivityDescriptor} from "../../models";

@Component({
  tag: 'elsa-workflow-editor',
  styleUrl: 'elsa-workflow-editor.scss',
})
export class ElsaWorkflowEditor {

  private canvas: HTMLElsaCanvasElement;
  private container: HTMLDivElement;
  private activityPicker: HTMLElsaActivityPickerElement;

  @Listen('resize', {target: 'window'})
  async handResize() {
    await this.updateLayout();
  }

  async componentDidLoad() {
    this.activityPicker.graph = await this.canvas.getGraph();
  }

  private updateLayout = async () => {
    await this.canvas.updateLayout();
  };

  private updateContainerLayout = async (panelClassName: string, panelExpanded: boolean) => {

    if (panelExpanded)
      this.container.classList.remove(panelClassName);
    else
      this.container.classList.toggle(panelClassName, true);

    await this.updateLayout();
  }

  private onActivityPickerPanelStateChanged = async (e: PanelStateChangedArgs) => await this.updateContainerLayout('activity-picker-closed', e.expanded)
  private onTriggerContainerPanelStateChanged = async (e: PanelStateChangedArgs) => await this.updateContainerLayout('trigger-container-closed', e.expanded)

  private static onDragOver(e: DragEvent) {
    e.preventDefault();
  }

  private async onDrop(e: DragEvent) {
    const json = e.dataTransfer.getData('activity-descriptor');
    const activityDescriptor: ActivityDescriptor = JSON.parse(json);
    const graph = await this.canvas.getGraph();

    const node =
      graph.createNode({
        shape: 'activity',
        text: activityDescriptor.displayName,
        x: e.offsetX,
        y: e.offsetY,
        ports: [
          {
            id: 'inbound1',
            group: 'in',
            attrs:{
              text:{
                text: 'In'
              }
            }
          },
          {
            id: 'outbound1',
            group: 'out',
            attrs:{
              text:{
                text: 'Done'
              }
            }
          }
        ]
      });

    graph.addNode(node);
  }

  render() {
    return (
      <div class="absolute top-0 left-0 bottom-0 right-0" ref={el => this.container = el}>
        <elsa-panel class="elsa-activity-picker-container"
                    onExpandedStateChanged={e => this.onActivityPickerPanelStateChanged(e.detail)}>
          <elsa-activity-picker ref={el => this.activityPicker = el}/>
        </elsa-panel>
        <elsa-panel class="elsa-trigger-container"
                    onExpandedStateChanged={e => this.onTriggerContainerPanelStateChanged(e.detail)}
                    orientation={PanelOrientation.Horizontal}>
          <elsa-trigger-container/>
        </elsa-panel>
        <elsa-canvas class="absolute" ref={el => this.canvas = el} onDragOver={e => ElsaWorkflowEditor.onDragOver(e)}
                     onDrop={e => this.onDrop(e)}/>
        <elsa-slide-over-panel/>
      </div>
    );
  }
}
