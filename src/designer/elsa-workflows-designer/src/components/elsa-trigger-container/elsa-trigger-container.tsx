import {Component, h, State} from '@stencil/core';
import {Trigger, TriggerDescriptor} from "../../models";
import {TriggerView} from "./elsa-trigger";

@Component({
  tag: 'elsa-trigger-container',
})
export class ElsaTriggerContainer {
  @State() triggers: Array<Trigger> = [];
  @State() triggerDescriptors: Array<TriggerDescriptor> = [];

  static onDragOver(e: DragEvent) {
    const isTrigger = e.dataTransfer.types.indexOf('trigger-descriptor') >= 0;

    if (isTrigger)
      e.preventDefault();
  }

  public render() {
    return (
      <div class="absolute left-0 top-0 right-0 bottom-0 overflow-auto" onDragOver={e => ElsaTriggerContainer.onDragOver(e)}
           onDrop={e => this.onDrop(e)}>
        {this.renderTriggers()}
      </div>
    );
  }

  private onDrop(e: DragEvent) {
    const json = e.dataTransfer.getData('trigger-descriptor');
    const triggerDescriptor = JSON.parse(json);

    const trigger: Trigger = {
      id: this.triggers.length.toString()
    };

    this.triggers = [...this.triggers, trigger];
    this.triggerDescriptors = [...this.triggerDescriptors, triggerDescriptor];
  }

  private renderTriggers() {
    const triggers = this.triggers;

    if (triggers.length == 0) {
      return <div>
        <div class="border-2 border-dashed m-4 p-4 text-center">
          <h2 class="text-lg font-medium text-gray-900">Triggers</h2>
          <p
            class="mt-1 text-sm text-gray-600">Place your workflow triggers here to automatically invoke your workflow when a trigger executed.</p>
        </div>
      </div>
    }

    return <div class="flex p-4 pt-8 space-x-4">
      {this.triggers.map((trigger, index) => (
        <div class="flex-none">
          <TriggerView trigger={trigger} triggerDescriptor={this.triggerDescriptors[index]}/>
        </div>
      ))}
    </div>
  }

}
