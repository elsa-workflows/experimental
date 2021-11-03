import {Component, Listen, h} from "@stencil/core";
import {PanelOrientation, PanelStateChangedArgs} from "../elsa-panel/models";

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

  private onActivityPickerPanelStateChanged = async (e: PanelStateChangedArgs) => {

    if (e.expanded)
      this.container.classList.remove('activity-picker-closed');
    else
      this.container.classList.toggle('activity-picker-closed', true);

    await this.updateLayout();
  }

  private onTriggerContainerPanelStateChanged = async (e: PanelStateChangedArgs) => {

    if (e.expanded)
      this.container.classList.remove('trigger-container-closed');
    else
      this.container.classList.toggle('trigger-container-closed', true);

    await this.updateLayout();
  }

  private updateLayout = async () => {
    await this.canvas.updateLayout();
  };

  render() {
    return (
      <div class="absolute top-0 left-0 bottom-0 right-0" ref={el => this.container = el}>
        <elsa-panel class="elsa-activity-picker-container" onExpandedStateChanged={e => this.onActivityPickerPanelStateChanged(e.detail)}>
          <elsa-activity-picker ref={el => this.activityPicker = el}/>
        </elsa-panel>
        <elsa-panel class="elsa-trigger-container" onExpandedStateChanged={e => this.onTriggerContainerPanelStateChanged(e.detail)}
                    orientation={PanelOrientation.Horizontal}>
          <elsa-trigger-container/>
        </elsa-panel>
        <elsa-canvas class="absolute top-0 right-0 bottom-0" ref={el => this.canvas = el}/>
      </div>
    );
  }
}
