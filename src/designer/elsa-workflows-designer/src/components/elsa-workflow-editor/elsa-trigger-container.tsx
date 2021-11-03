import {Component, h, Method, Element, State} from '@stencil/core';
import './shapes';
import {Trigger} from "../../models/core";

@Component({
  tag: 'elsa-trigger-container',
})
export class ElsaTriggerContainer {
  @State() triggers: Array<Trigger> = [];

  static onDragOver(e: DragEvent) {
    const isTrigger = e.dataTransfer.types.indexOf('trigger-descriptor') >= 0;

    if (isTrigger)
      e.preventDefault();
  }

  private onDrop(e: DragEvent) {
    const json = e.dataTransfer.getData('trigger-descriptor');
    const triggerDescriptor = JSON.parse(json);

    const trigger: Trigger = {
      id: this.triggers.length.toString()
    };

    this.triggers = [...this.triggers, trigger];
  }

  render() {
    return (
      <div class="absolute left-0 top-0 right-0 bottom-0" onDragOver={e => ElsaTriggerContainer.onDragOver(e)}
           onDrop={e => this.onDrop(e)}>
        {this.triggers.map(trigger => (
          <div>
            {trigger.id}
          </div>
        ))}
      </div>
    );
  }


}
