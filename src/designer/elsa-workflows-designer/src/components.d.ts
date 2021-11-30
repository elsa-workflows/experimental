/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { Graph } from "@antv/x6";
import { Activity, ActivityDescriptor, ActivityEditRequestArgs, ActivityInput } from "./models";
import { AddActivityArgs } from "./components/designer/elsa-canvas/elsa-canvas";
import { AddActivityArgs as AddActivityArgs1 } from "./components/designer/elsa-canvas/elsa-canvas";
import { PanelOrientation, PanelStateChangedArgs } from "./components/designer/elsa-panel/models";
import { ActionDefinition, TabDefinition } from "./components/designer/elsa-slide-over-panel/models";
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
    interface ElsaFreeFlowchart {
        "addActivity": (args: AddActivityArgs) => Promise<void>;
        "updateLayout": () => Promise<void>;
    }
    interface ElsaPanel {
        "orientation": PanelOrientation;
    }
    interface ElsaShell {
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
    interface HTMLElsaShellElement extends Components.ElsaShell, HTMLStencilElement {
    }
    var HTMLElsaShellElement: {
        prototype: HTMLElsaShellElement;
        new (): HTMLElsaShellElement;
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
    interface HTMLElementTagNameMap {
        "elsa-activity-picker": HTMLElsaActivityPickerElement;
        "elsa-activity-properties-editor": HTMLElsaActivityPropertiesEditorElement;
        "elsa-canvas": HTMLElsaCanvasElement;
        "elsa-free-flowchart": HTMLElsaFreeFlowchartElement;
        "elsa-panel": HTMLElsaPanelElement;
        "elsa-shell": HTMLElsaShellElement;
        "elsa-slide-over-panel": HTMLElsaSlideOverPanelElement;
        "elsa-trigger-container": HTMLElsaTriggerContainerElement;
        "elsa-workflow-editor": HTMLElsaWorkflowEditorElement;
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
        "onActivityUpdated"?: (event: CustomEvent<Activity>) => void;
    }
    interface ElsaCanvas {
    }
    interface ElsaFreeFlowchart {
        "onActivityEditRequested"?: (event: CustomEvent<ActivityEditRequestArgs>) => void;
    }
    interface ElsaPanel {
        "onExpandedStateChanged"?: (event: CustomEvent<PanelStateChangedArgs>) => void;
        "orientation"?: PanelOrientation;
    }
    interface ElsaShell {
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
    }
    interface IntrinsicElements {
        "elsa-activity-picker": ElsaActivityPicker;
        "elsa-activity-properties-editor": ElsaActivityPropertiesEditor;
        "elsa-canvas": ElsaCanvas;
        "elsa-free-flowchart": ElsaFreeFlowchart;
        "elsa-panel": ElsaPanel;
        "elsa-shell": ElsaShell;
        "elsa-slide-over-panel": ElsaSlideOverPanel;
        "elsa-trigger-container": ElsaTriggerContainer;
        "elsa-workflow-editor": ElsaWorkflowEditor;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "elsa-activity-picker": LocalJSX.ElsaActivityPicker & JSXBase.HTMLAttributes<HTMLElsaActivityPickerElement>;
            "elsa-activity-properties-editor": LocalJSX.ElsaActivityPropertiesEditor & JSXBase.HTMLAttributes<HTMLElsaActivityPropertiesEditorElement>;
            "elsa-canvas": LocalJSX.ElsaCanvas & JSXBase.HTMLAttributes<HTMLElsaCanvasElement>;
            "elsa-free-flowchart": LocalJSX.ElsaFreeFlowchart & JSXBase.HTMLAttributes<HTMLElsaFreeFlowchartElement>;
            "elsa-panel": LocalJSX.ElsaPanel & JSXBase.HTMLAttributes<HTMLElsaPanelElement>;
            "elsa-shell": LocalJSX.ElsaShell & JSXBase.HTMLAttributes<HTMLElsaShellElement>;
            "elsa-slide-over-panel": LocalJSX.ElsaSlideOverPanel & JSXBase.HTMLAttributes<HTMLElsaSlideOverPanelElement>;
            "elsa-trigger-container": LocalJSX.ElsaTriggerContainer & JSXBase.HTMLAttributes<HTMLElsaTriggerContainerElement>;
            "elsa-workflow-editor": LocalJSX.ElsaWorkflowEditor & JSXBase.HTMLAttributes<HTMLElsaWorkflowEditorElement>;
        }
    }
}
