/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { Graph } from "@antv/x6";
import { ActionDefinition, ActionInvokedArgs, Activity, ActivityDescriptor, ActivityEditRequestArgs, GraphUpdatedArgs, TabDefinition, Workflow } from "./models";
import { ActivityUpdatedArgs, DeleteActivityRequestedArgs } from "./components/designer/elsa-workflow-editor/elsa-activity-properties-editor";
import { AddActivityArgs } from "./components/designer/elsa-canvas/elsa-canvas";
import { TabChangedArgs } from "./components/designer/elsa-form-panel/elsa-form-panel";
import { AddActivityArgs as AddActivityArgs1 } from "./components/designer/elsa-canvas/elsa-canvas";
import { PanelPosition, PanelStateChangedArgs } from "./components/designer/elsa-panel/models";
import { WorkflowUpdatedArgs } from "./components/designer/elsa-workflow-editor/elsa-workflow-editor";
export namespace Components {
    interface ElsaActivityPicker {
        "activityDescriptors": Array<ActivityDescriptor>;
        "graph": Graph;
    }
    interface ElsaActivityPropertiesEditor {
        "activity"?: Activity;
        "activityDescriptors": Array<ActivityDescriptor>;
        "hide": () => Promise<void>;
        "show": () => Promise<void>;
    }
    interface ElsaCanvas {
        "addActivity": (args: AddActivityArgs) => Promise<void>;
        "updateLayout": () => Promise<void>;
    }
    interface ElsaFormPanel {
        "actions": Array<ActionDefinition>;
        "headerText": string;
        "selectedTabIndex"?: number;
        "tabs": Array<TabDefinition>;
    }
    interface ElsaFreeFlowchart {
        "addActivity": (args: AddActivityArgs) => Promise<void>;
        "updateLayout": () => Promise<void>;
    }
    interface ElsaPanel {
        "position": PanelPosition;
    }
    interface ElsaServerShell {
        "serverUrl": string;
    }
    interface ElsaSlideOverPanel {
        "actions": Array<ActionDefinition>;
        "expand": boolean;
        "headerText": string;
        "hide": () => Promise<void>;
        "selectedTab"?: TabDefinition;
        "show": () => Promise<void>;
        "tabs": Array<TabDefinition>;
    }
    interface ElsaTriggerContainer {
    }
    interface ElsaWorkflowEditor {
        "activityDescriptors": Array<ActivityDescriptor>;
        "workflow": Workflow;
    }
    interface ElsaWorkflowPublishButton {
        "publishing": boolean;
    }
    interface ElsaWorkflowToolbar {
    }
}
declare global {
    interface HTMLElsaActivityPickerElement extends Components.ElsaActivityPicker, HTMLStencilElement {
    }
    var HTMLElsaActivityPickerElement: {
        prototype: HTMLElsaActivityPickerElement;
        new (): HTMLElsaActivityPickerElement;
    };
    interface HTMLElsaActivityPropertiesEditorElement extends Components.ElsaActivityPropertiesEditor, HTMLStencilElement {
    }
    var HTMLElsaActivityPropertiesEditorElement: {
        prototype: HTMLElsaActivityPropertiesEditorElement;
        new (): HTMLElsaActivityPropertiesEditorElement;
    };
    interface HTMLElsaCanvasElement extends Components.ElsaCanvas, HTMLStencilElement {
    }
    var HTMLElsaCanvasElement: {
        prototype: HTMLElsaCanvasElement;
        new (): HTMLElsaCanvasElement;
    };
    interface HTMLElsaFormPanelElement extends Components.ElsaFormPanel, HTMLStencilElement {
    }
    var HTMLElsaFormPanelElement: {
        prototype: HTMLElsaFormPanelElement;
        new (): HTMLElsaFormPanelElement;
    };
    interface HTMLElsaFreeFlowchartElement extends Components.ElsaFreeFlowchart, HTMLStencilElement {
    }
    var HTMLElsaFreeFlowchartElement: {
        prototype: HTMLElsaFreeFlowchartElement;
        new (): HTMLElsaFreeFlowchartElement;
    };
    interface HTMLElsaPanelElement extends Components.ElsaPanel, HTMLStencilElement {
    }
    var HTMLElsaPanelElement: {
        prototype: HTMLElsaPanelElement;
        new (): HTMLElsaPanelElement;
    };
    interface HTMLElsaServerShellElement extends Components.ElsaServerShell, HTMLStencilElement {
    }
    var HTMLElsaServerShellElement: {
        prototype: HTMLElsaServerShellElement;
        new (): HTMLElsaServerShellElement;
    };
    interface HTMLElsaSlideOverPanelElement extends Components.ElsaSlideOverPanel, HTMLStencilElement {
    }
    var HTMLElsaSlideOverPanelElement: {
        prototype: HTMLElsaSlideOverPanelElement;
        new (): HTMLElsaSlideOverPanelElement;
    };
    interface HTMLElsaTriggerContainerElement extends Components.ElsaTriggerContainer, HTMLStencilElement {
    }
    var HTMLElsaTriggerContainerElement: {
        prototype: HTMLElsaTriggerContainerElement;
        new (): HTMLElsaTriggerContainerElement;
    };
    interface HTMLElsaWorkflowEditorElement extends Components.ElsaWorkflowEditor, HTMLStencilElement {
    }
    var HTMLElsaWorkflowEditorElement: {
        prototype: HTMLElsaWorkflowEditorElement;
        new (): HTMLElsaWorkflowEditorElement;
    };
    interface HTMLElsaWorkflowPublishButtonElement extends Components.ElsaWorkflowPublishButton, HTMLStencilElement {
    }
    var HTMLElsaWorkflowPublishButtonElement: {
        prototype: HTMLElsaWorkflowPublishButtonElement;
        new (): HTMLElsaWorkflowPublishButtonElement;
    };
    interface HTMLElsaWorkflowToolbarElement extends Components.ElsaWorkflowToolbar, HTMLStencilElement {
    }
    var HTMLElsaWorkflowToolbarElement: {
        prototype: HTMLElsaWorkflowToolbarElement;
        new (): HTMLElsaWorkflowToolbarElement;
    };
    interface HTMLElementTagNameMap {
        "elsa-activity-picker": HTMLElsaActivityPickerElement;
        "elsa-activity-properties-editor": HTMLElsaActivityPropertiesEditorElement;
        "elsa-canvas": HTMLElsaCanvasElement;
        "elsa-form-panel": HTMLElsaFormPanelElement;
        "elsa-free-flowchart": HTMLElsaFreeFlowchartElement;
        "elsa-panel": HTMLElsaPanelElement;
        "elsa-server-shell": HTMLElsaServerShellElement;
        "elsa-slide-over-panel": HTMLElsaSlideOverPanelElement;
        "elsa-trigger-container": HTMLElsaTriggerContainerElement;
        "elsa-workflow-editor": HTMLElsaWorkflowEditorElement;
        "elsa-workflow-publish-button": HTMLElsaWorkflowPublishButtonElement;
        "elsa-workflow-toolbar": HTMLElsaWorkflowToolbarElement;
    }
}
declare namespace LocalJSX {
    interface ElsaActivityPicker {
        "activityDescriptors"?: Array<ActivityDescriptor>;
        "graph"?: Graph;
    }
    interface ElsaActivityPropertiesEditor {
        "activity"?: Activity;
        "activityDescriptors"?: Array<ActivityDescriptor>;
        "onActivityUpdated"?: (event: CustomEvent<ActivityUpdatedArgs>) => void;
        "onDeleteActivityRequested"?: (event: CustomEvent<DeleteActivityRequestedArgs>) => void;
    }
    interface ElsaCanvas {
    }
    interface ElsaFormPanel {
        "actions"?: Array<ActionDefinition>;
        "headerText"?: string;
        "onActionInvoked"?: (event: CustomEvent<ActionInvokedArgs>) => void;
        "onSelectedTabIndexChanged"?: (event: CustomEvent<TabChangedArgs>) => void;
        "onSubmitted"?: (event: CustomEvent<FormData>) => void;
        "selectedTabIndex"?: number;
        "tabs"?: Array<TabDefinition>;
    }
    interface ElsaFreeFlowchart {
        "onActivityEditRequested"?: (event: CustomEvent<ActivityEditRequestArgs>) => void;
        "onGraphUpdated"?: (event: CustomEvent<GraphUpdatedArgs>) => void;
    }
    interface ElsaPanel {
        "onExpandedStateChanged"?: (event: CustomEvent<PanelStateChangedArgs>) => void;
        "position"?: PanelPosition;
    }
    interface ElsaServerShell {
        "serverUrl"?: string;
    }
    interface ElsaSlideOverPanel {
        "actions"?: Array<ActionDefinition>;
        "expand"?: boolean;
        "headerText"?: string;
        "onCollapsed"?: (event: CustomEvent<any>) => void;
        "onSubmitted"?: (event: CustomEvent<FormData>) => void;
        "selectedTab"?: TabDefinition;
        "tabs"?: Array<TabDefinition>;
    }
    interface ElsaTriggerContainer {
    }
    interface ElsaWorkflowEditor {
        "activityDescriptors"?: Array<ActivityDescriptor>;
        "onWorkflowUpdated"?: (event: CustomEvent<WorkflowUpdatedArgs>) => void;
        "workflow"?: Workflow;
    }
    interface ElsaWorkflowPublishButton {
        "onExportClicked"?: (event: CustomEvent<any>) => void;
        "onImportClicked"?: (event: CustomEvent<File>) => void;
        "onPublishClicked"?: (event: CustomEvent<any>) => void;
        "onUnPublishClicked"?: (event: CustomEvent<any>) => void;
        "publishing"?: boolean;
    }
    interface ElsaWorkflowToolbar {
    }
    interface IntrinsicElements {
        "elsa-activity-picker": ElsaActivityPicker;
        "elsa-activity-properties-editor": ElsaActivityPropertiesEditor;
        "elsa-canvas": ElsaCanvas;
        "elsa-form-panel": ElsaFormPanel;
        "elsa-free-flowchart": ElsaFreeFlowchart;
        "elsa-panel": ElsaPanel;
        "elsa-server-shell": ElsaServerShell;
        "elsa-slide-over-panel": ElsaSlideOverPanel;
        "elsa-trigger-container": ElsaTriggerContainer;
        "elsa-workflow-editor": ElsaWorkflowEditor;
        "elsa-workflow-publish-button": ElsaWorkflowPublishButton;
        "elsa-workflow-toolbar": ElsaWorkflowToolbar;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "elsa-activity-picker": LocalJSX.ElsaActivityPicker & JSXBase.HTMLAttributes<HTMLElsaActivityPickerElement>;
            "elsa-activity-properties-editor": LocalJSX.ElsaActivityPropertiesEditor & JSXBase.HTMLAttributes<HTMLElsaActivityPropertiesEditorElement>;
            "elsa-canvas": LocalJSX.ElsaCanvas & JSXBase.HTMLAttributes<HTMLElsaCanvasElement>;
            "elsa-form-panel": LocalJSX.ElsaFormPanel & JSXBase.HTMLAttributes<HTMLElsaFormPanelElement>;
            "elsa-free-flowchart": LocalJSX.ElsaFreeFlowchart & JSXBase.HTMLAttributes<HTMLElsaFreeFlowchartElement>;
            "elsa-panel": LocalJSX.ElsaPanel & JSXBase.HTMLAttributes<HTMLElsaPanelElement>;
            "elsa-server-shell": LocalJSX.ElsaServerShell & JSXBase.HTMLAttributes<HTMLElsaServerShellElement>;
            "elsa-slide-over-panel": LocalJSX.ElsaSlideOverPanel & JSXBase.HTMLAttributes<HTMLElsaSlideOverPanelElement>;
            "elsa-trigger-container": LocalJSX.ElsaTriggerContainer & JSXBase.HTMLAttributes<HTMLElsaTriggerContainerElement>;
            "elsa-workflow-editor": LocalJSX.ElsaWorkflowEditor & JSXBase.HTMLAttributes<HTMLElsaWorkflowEditorElement>;
            "elsa-workflow-publish-button": LocalJSX.ElsaWorkflowPublishButton & JSXBase.HTMLAttributes<HTMLElsaWorkflowPublishButtonElement>;
            "elsa-workflow-toolbar": LocalJSX.ElsaWorkflowToolbar & JSXBase.HTMLAttributes<HTMLElsaWorkflowToolbarElement>;
        }
    }
}
