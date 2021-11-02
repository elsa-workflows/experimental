import {Component, h, Event, EventEmitter, State, Prop, Watch} from "@stencil/core";
import {PanelOrientation, PanelStateChangedArgs} from "./models";

@Component({
  tag: 'elsa-panel',
  styleUrl: 'elsa-panel.scss',
})
export class ElsaPanel {
  @Prop() size: number = 300;
  @Prop() orientation: PanelOrientation = PanelOrientation.Vertical;
  @Event() expandedStateChanged: EventEmitter<PanelStateChangedArgs>;
  @State() isExpanded: boolean = true;

  private onToggleClick = () => {
    this.isExpanded = !this.isExpanded;
    this.expandedStateChanged.emit({expanded: this.isExpanded});
  };

  render() {
    const size = this.isExpanded ? this.size : 0;
    const orientation = this.orientation;
    const containerCssClass = orientation == PanelOrientation.Vertical ? 'panel-orientation-v left-0 top-0 bottom-0 border-r' : 'panel-orientation-h left-0 top-0 right-0 border-b';
    const toggleCssClass = orientation == PanelOrientation.Vertical ? 'panel-toggle-v' : 'panel-toggle-h';
    const panelStyle = orientation == PanelOrientation.Vertical ? {width: `${size}px`} : {height: `${size}px`};

    return (
      <div class={`panel absolute transition-all duration-200 ease-in-out ${containerCssClass}`} style={panelStyle}>

        <div class="panel-content-container">
          <slot/>
        </div>

        <div class={`text-white ${toggleCssClass}`} onClick={() => this.onToggleClick()}>
          <svg class="h-6 w-6 text-gray-700" width="24" height="24" viewBox="0 0 24 24" stroke-width="2"
               stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
            <path stroke="none" d="M0 0h24v24H0z"/>
            <polyline points="9 6 15 12 9 18"/>
          </svg>
        </div>
      </div>
    );
  }
}
