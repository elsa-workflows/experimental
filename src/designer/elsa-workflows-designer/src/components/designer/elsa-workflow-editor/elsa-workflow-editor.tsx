import {Component, h, Listen, State} from "@stencil/core";
import {PanelOrientation, PanelStateChangedArgs} from "../elsa-panel/models";
import {Activity, ActivityDescriptor, ActivityEditRequestArgs} from "../../../models";
import WorkflowEditorTunnel, {WorkflowEditorState} from "./state";
import {Container} from "typedi";
import {ElsaApiClientProvider} from "../../../services/elsa-api-client-provider";

@Component({
  tag: 'elsa-workflow-editor',
  styleUrl: 'elsa-workflow-editor.scss',
})
export class ElsaWorkflowEditor {

  private canvas: HTMLElsaCanvasElement;
  private container: HTMLDivElement;
  private activityPicker: HTMLElsaActivityPickerElement;
  private activityPropertiesEditor: HTMLElsaActivityPropertiesEditorElement;
  private activityDescriptors: Array<ActivityDescriptor> = [];
  private applyActivityChanges: (activity: Activity) => void;

  @State() private activityUnderEdit?: Activity;

  @Listen('resize', {target: 'window'})
  async handResize() {
    await this.updateLayout();
  }

  @Listen('collapsed')
  async handPanelCollapsed() {
    this.activityUnderEdit = null;
  }

  @Listen('activityEditRequested')
  async handleActivityEditRequested(e: CustomEvent<ActivityEditRequestArgs>) {
    this.activityUnderEdit = e.detail.activity;
    this.applyActivityChanges = e.detail.applyChanges;
  }

  async componentWillLoad() {
    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    const client = await elsaClientProvider.getClient();
    this.activityDescriptors = await client.activityDescriptorsApi.list();
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

    await this.canvas.addActivity({descriptor: activityDescriptor, x: e.offsetX, y: e.offsetY});
  }

  private onActivityUpdated = (e: CustomEvent<Activity>) => {
    this.activityUnderEdit = null;
    const updatedActivity = e.detail;
    this.applyActivityChanges(updatedActivity);
  }

  render() {

    const tunnelState: WorkflowEditorState = {
      workflowDefinitionId: null,
      activityDescriptors: this.activityDescriptors
    };

    const activityUnderEdit = this.activityUnderEdit;

    return (
      <WorkflowEditorTunnel.Provider state={tunnelState}>
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
          <elsa-canvas class="absolute" ref={el => this.canvas = el}
                       onDragOver={e => ElsaWorkflowEditor.onDragOver(e)}
                       onDrop={e => this.onDrop(e)}/>
          <elsa-activity-properties-editor activity={activityUnderEdit}
                                           onActivityUpdated={e => this.onActivityUpdated(e)}
                                           ref={el => this.activityPropertiesEditor = el}/>
        </div>
      </WorkflowEditorTunnel.Provider>
    );
  }
}
