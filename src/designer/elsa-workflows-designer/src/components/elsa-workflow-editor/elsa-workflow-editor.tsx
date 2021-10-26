import {Component, h} from "@stencil/core";
import {Shape, Addon, Graph} from '@antv/x6';

@Component({
  tag: 'elsa-workflow-editor',
  styleUrl: 'elsa-workflow-editor.scss',
})
export class ElsaWorkflowEditor {

  private canvas: HTMLElsaCanvasElement;
  private container: HTMLDivElement;
  private graph: Graph;
  private canvasContainer: HTMLDivElement;

  async componentDidLoad() {
    const graph = this.graph = await this.canvas.getGraph();
  }

  private onToggleClick = () => {
    this.container.classList.toggle('activity-picker-closed');

    const width = this.canvasContainer.scrollWidth;
    const height = this.canvasContainer.scrollHeight;
    console.debug({width, height});
    this.graph.resize(width, height);
  };

  render() {
    return (
      <div class="absolute top-0 left-0 bottom-0 right-0" ref={el => this.container = el}>
        <div class="activity-picker absolute left-0 top-0 bottom-0 transition-all duration-200 ease-in-out">

          <nav class="flex-1 px-2 space-y-1 font-sans text-sm text-gray-600">
            <div class="space-y-1">
              <button type="button"
                      class="text-gray-600 hover:bg-gray-50 hover:text-gray-900 group w-full flex items-center pr-2 py-2 text-left text-sm font-medium rounded-md focus:outline-none">
                <svg
                  class="text-gray-300 mr-2 flex-shrink-0 h-5 w-5 transform group-hover:text-gray-400 transition-colors ease-in-out duration-150"
                  viewBox="0 0 20 20" aria-hidden="true">
                  <path d="M6 6L14 10L6 14V6Z" fill="currentColor"/>
                </svg>
                Primitives
              </button>

              <div class="space-y-1"
                   id="sub-menu-1" style={{display: "block"}}>

                {[...Array(5)].map(x => (
                  <div class="w-full flex items-center pl-10 pr-2 py-2">
                    <div class="border border-solid rounded-xl border-blue-400 p-2 cursor-move">
                      HTTP Trigger and more and a very long text to break the layout
                    </div>
                  </div>
                ))}

              </div>
            </div>


          </nav>

          <div class="activity-picker-toggle panel-v-toggle text-white" onClick={() => this.onToggleClick()}>
            toggle
          </div>
        </div>
        <div class="canvas-container absolute top-0 right-0 bottom-0" ref={el => this.canvasContainer = el}>
          <elsa-canvas ref={el => this.canvas = el}/>
        </div>
      </div>
    );
  }
}
