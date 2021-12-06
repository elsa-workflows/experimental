import {Component, h, Listen, Prop, State, Event, EventEmitter, Method} from "@stencil/core";
import {debounce} from 'lodash';
import {v4 as uuid} from 'uuid';
import {Container} from "typedi";
import {PanelPosition, PanelStateChangedArgs} from "../elsa-panel/models";
import {
  Activity,
  ActivityDescriptor,
  ActivityEditRequestArgs,
  GraphUpdatedArgs,
  TriggerDescriptor,
  Workflow
} from "../../../models";
import WorkflowEditorTunnel, {WorkflowEditorState} from "./state";
import ShellTunnel from "../../shell/elsa-server-shell/state";
import {
  ActivityUpdatedArgs,
  DeleteActivityRequestedArgs
} from "./elsa-activity-properties-editor";
import {ActivityDriverRegistry} from "../../../services";

export interface WorkflowUpdatedArgs {
  workflow: Workflow;
}

@Component({
  tag: 'elsa-workflow-editor',
  styleUrl: 'elsa-workflow-editor.scss',
})
export class ElsaWorkflowEditor {

  private canvas: HTMLElsaCanvasElement;
  private container: HTMLDivElement;
  private toolbox: HTMLElsaToolboxElement;
  private activityPropertiesEditor: HTMLElsaActivityPropertiesEditorElement;
  private applyActivityChanges: (activity: Activity) => void;
  private deleteActivity: (activity: Activity) => void;
  private readonly saveChangesDebounced: (e: CustomEvent<GraphUpdatedArgs>) => void;

  constructor() {
    this.saveChangesDebounced = debounce(this.saveChanges, 1000);
  }

  @Prop() activityDescriptors: Array<ActivityDescriptor> = [];
  @Prop() triggerDescriptors: Array<TriggerDescriptor> = [];

  @Prop() workflow: Workflow = {
    root: null,
    metadata: {identity: {id: uuid(), version: 1}, publication: {isLatest: true, isPublished: false}},
    triggers: []
  };

  @Event() workflowUpdated: EventEmitter<WorkflowUpdatedArgs>

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

  @Listen('graphUpdated')
  handleGraphUpdated(e: CustomEvent<GraphUpdatedArgs>) {
    this.saveChangesDebounced(e);
  }

  @Method()
  async registerActivityDrivers(register: (registry: ActivityDriverRegistry) => void): Promise<void> {
    const registry = Container.get(ActivityDriverRegistry);
    register(registry);
  }

  saveChanges = (e: CustomEvent<GraphUpdatedArgs>) => {
    const root = e.detail.exportGraph();
    const workflow = this.workflow;

    workflow.root = root;

    this.workflowUpdated.emit({workflow});
  };

  updateLayout = async () => {
    await this.canvas.updateLayout();
  };

  updateContainerLayout = async (panelClassName: string, panelExpanded: boolean) => {

    if (panelExpanded)
      this.container.classList.remove(panelClassName);
    else
      this.container.classList.toggle(panelClassName, true);

    await this.updateLayout();
  }

  onActivityPickerPanelStateChanged = async (e: PanelStateChangedArgs) => await this.updateContainerLayout('activity-picker-closed', e.expanded)
  onTriggerContainerPanelStateChanged = async (e: PanelStateChangedArgs) => await this.updateContainerLayout('trigger-container-closed', e.expanded)
  onActivityEditorPanelStateChanged = async (e: PanelStateChangedArgs) => await this.updateContainerLayout('activity-editor-closed', e.expanded)

  static onDragOver(e: DragEvent) {
    e.preventDefault();
  }

  async onDrop(e: DragEvent) {
    const json = e.dataTransfer.getData('activity-descriptor');
    const activityDescriptor: ActivityDescriptor = JSON.parse(json);

    await this.canvas.addActivity({descriptor: activityDescriptor, x: e.offsetX, y: e.offsetY});
  }

  onActivityUpdated = (e: CustomEvent<ActivityUpdatedArgs>) => {
    const updatedActivity = e.detail.activity;
    this.applyActivityChanges(updatedActivity);
  }

  onDeleteActivityRequested(e: CustomEvent<DeleteActivityRequestedArgs>) {
    this.deleteActivity(e.detail.activity);
    this.activityUnderEdit = null;
  }

  public render() {

    const tunnelState: WorkflowEditorState = {
      workflowDefinitionId: null,
      activityDescriptors: this.activityDescriptors,
      triggerDescriptors: this.triggerDescriptors
    };

    const activityUnderEdit = this.activityUnderEdit;

    return (
      <WorkflowEditorTunnel.Provider state={tunnelState}>
        <div class="absolute inset-0" ref={el => this.container = el}>
          <elsa-panel class="elsa-activity-picker-container"
                      position={PanelPosition.Left}
                      onExpandedStateChanged={e => this.onActivityPickerPanelStateChanged(e.detail)}>
            <elsa-toolbox ref={el => this.toolbox = el}/>
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
                                             onActivityUpdated={e => this.onActivityUpdated(e)}
                                             onDeleteActivityRequested={e => this.onDeleteActivityRequested(e)}
                                             ref={el => this.activityPropertiesEditor = el}/>
          </elsa-panel>

        </div>
      </WorkflowEditorTunnel.Provider>
    );
  }
}

ShellTunnel.injectProps(ElsaWorkflowEditor, ['activityDescriptors', 'triggerDescriptors']);
