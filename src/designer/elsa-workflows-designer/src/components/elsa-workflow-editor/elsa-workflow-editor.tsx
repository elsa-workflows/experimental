import {Component, Listen, h} from "@stencil/core";
import {ActivityPickerStateChangedArgs} from "../elsa-activity-picker/elsa-activity-picker";

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

  private onActivityPickerStateChanged = async (e: ActivityPickerStateChangedArgs) => {

    if (e.expanded)
      this.container.classList.remove('activity-picker-closed');
    else
      this.container.classList.toggle('activity-picker-closed', true);

    await this.updateLayout();
  }

  private updateLayout = async () => {
    await this.canvas.updateLayout();
  };

  render() {
    return (
      <div class="absolute top-0 left-0 bottom-0 right-0" ref={el => this.container = el}>
        <elsa-activity-picker ref={el => this.activityPicker = el}
                              onExpandedStateChanged={(e) => this.onActivityPickerStateChanged(e.detail)}/>
        <elsa-canvas class="absolute top-0 right-0 bottom-0" ref={el => this.canvas = el}/>
      </div>
    );
  }


}
