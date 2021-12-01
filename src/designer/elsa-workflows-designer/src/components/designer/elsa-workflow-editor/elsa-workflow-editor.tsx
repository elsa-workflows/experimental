import {Component, h, Listen, Prop, State} from "@stencil/core";
import {PanelPosition, PanelStateChangedArgs} from "../elsa-panel/models";
import {Activity, ActivityDescriptor, ActivityEditRequestArgs} from "../../../models";
import WorkflowEditorTunnel, {WorkflowEditorState} from "./state";
import ShellTunnel from "../elsa-shell/state";
import {ActivityPropertyChangedArgs, DeleteActivityRequestedArgs} from "./elsa-activity-properties-editor";

@Component({
  tag: 'elsa-workflow-editor',
  styleUrl: 'elsa-workflow-editor.scss',
})
export class ElsaWorkflowEditor {

  private canvas: HTMLElsaCanvasElement;
  private container: HTMLDivElement;
  private activityPicker: HTMLElsaActivityPickerElement;
  private activityPropertiesEditor: HTMLElsaActivityPropertiesEditorElement;
  private applyActivityChanges: (activity: Activity) => void;
  private deleteActivity: (activity: Activity) => void;

  @Prop() activityDescriptors: Array<ActivityDescriptor> = [];

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
    this.deleteActivity = e.detail.deleteActivity;
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
  private onActivityEditorPanelStateChanged = async (e: PanelStateChangedArgs) => await this.updateContainerLayout('activity-editor-closed', e.expanded)

  private static onDragOver(e: DragEvent) {
    e.preventDefault();
  }

  private async onDrop(e: DragEvent) {
    const json = e.dataTransfer.getData('activity-descriptor');
    const activityDescriptor: ActivityDescriptor = JSON.parse(json);

    await this.canvas.addActivity({descriptor: activityDescriptor, x: e.offsetX, y: e.offsetY});
  }

  private onActivityPropertyChanged = (e: CustomEvent<ActivityPropertyChangedArgs>) => {
    this.activityUnderEdit = null;
    const updatedActivity = e.detail.activity;
    this.applyActivityChanges(updatedActivity);
  }

  private onDeleteActivityRequested(e: CustomEvent<DeleteActivityRequestedArgs>) {
    this.deleteActivity(e.detail.activity);
    this.activityUnderEdit = null;
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
                      position={PanelPosition.Left}
                      onExpandedStateChanged={e => this.onActivityPickerPanelStateChanged(e.detail)}>
            <elsa-activity-picker ref={el => this.activityPicker = el}/>
          </elsa-panel>
          <elsa-panel class="elsa-trigger-container"
                      onExpandedStateChanged={e => this.onTriggerContainerPanelStateChanged(e.detail)}
                      position={PanelPosition.Top}>
            <elsa-trigger-container/>
          </elsa-panel>
          <elsa-canvas class="absolute" ref={el => this.canvas = el}
                       onDragOver={e => ElsaWorkflowEditor.onDragOver(e)}
                       onDrop={e => this.onDrop(e)}/>

          <elsa-panel class="elsa-activity-editor-container"
                      position={PanelPosition.Right}
                      onExpandedStateChanged={e => this.onActivityEditorPanelStateChanged(e.detail)}>
            <elsa-activity-properties-editor activity={activityUnderEdit}
                                             onActivityPropertyChanged={e => this.onActivityPropertyChanged(e)}
                                             onDeleteActivityRequested={e => this.onDeleteActivityRequested(e)}
                                             ref={el => this.activityPropertiesEditor = el}/>
          </elsa-panel>

        </div>
      </WorkflowEditorTunnel.Provider>
    );
  }
}

ShellTunnel.injectProps(ElsaWorkflowEditor, ['activityDescriptors']);
