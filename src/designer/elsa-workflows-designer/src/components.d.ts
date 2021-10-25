/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { Graph } from "@antv/x6";
export namespace Components {
    interface ElsaCanvas {
        "getGraph": () => Promise<Graph>;
    }
    interface ElsaWorkflowEditor {
    }
}
declare global {
    interface HTMLElsaCanvasElement extends Components.ElsaCanvas, HTMLStencilElement {
    }
    var HTMLElsaCanvasElement: {
        prototype: HTMLElsaCanvasElement;
        new (): HTMLElsaCanvasElement;
    };
    interface HTMLElsaWorkflowEditorElement extends Components.ElsaWorkflowEditor, HTMLStencilElement {
    }
    var HTMLElsaWorkflowEditorElement: {
        prototype: HTMLElsaWorkflowEditorElement;
        new (): HTMLElsaWorkflowEditorElement;
    };
    interface HTMLElementTagNameMap {
        "elsa-canvas": HTMLElsaCanvasElement;
        "elsa-workflow-editor": HTMLElsaWorkflowEditorElement;
    }
}
declare namespace LocalJSX {
    interface ElsaCanvas {
    }
    interface ElsaWorkflowEditor {
    }
    interface IntrinsicElements {
        "elsa-canvas": ElsaCanvas;
        "elsa-workflow-editor": ElsaWorkflowEditor;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "elsa-canvas": LocalJSX.ElsaCanvas & JSXBase.HTMLAttributes<HTMLElsaCanvasElement>;
            "elsa-workflow-editor": LocalJSX.ElsaWorkflowEditor & JSXBase.HTMLAttributes<HTMLElsaWorkflowEditorElement>;
        }
    }
}
