import {Component, h, State} from '@stencil/core';
import {v4 as uuid} from 'uuid';
import {Trigger, TriggerDescriptor} from "../../../models";
import {Container} from "typedi";
import {TriggerDriverRegistry} from "../../../services/trigger-driver-registry";

@Component({
  tag: 'elsa-trigger-container',
})
export class ElsaTriggerContainer {
  @State() triggers: Array<Trigger> = [];
  @State() triggerDescriptors: Array<TriggerDescriptor> = [];
  private renderedTriggers: Map<string, string> = new Map<string, string>();

  static onDragOver(e: DragEvent) {
    const isTrigger = e.dataTransfer.types.indexOf('trigger-descriptor') >= 0;

    if (isTrigger)
      e.preventDefault();
  }

  public componentWillRender() {
    const triggerDescriptors = this.triggerDescriptors;
    const triggers = this.triggers;
    const triggerDriverRegistry = Container.get(TriggerDriverRegistry);
    const renderedTriggers = new Map<string, string>();

    for (const trigger of triggers) {
      const triggerType = trigger.triggerType;
      const triggerDescriptor = triggerDescriptors.find(x => x.triggerType == triggerType);
      const driver = triggerDriverRegistry.createDriver(triggerType);
      const html = driver.display({triggerDescriptor: triggerDescriptor, trigger: trigger, displayType: 'designer'});

      renderedTriggers.set(trigger.id, html);
    }

    this.renderedTriggers = renderedTriggers;
  }

  public render() {
    return (
      <div class="absolute left-0 top-0 right-0 bottom-0 overflow-auto"
           onDragOver={e => ElsaTriggerContainer.onDragOver(e)}
           onDrop={e => this.onDrop(e)}>
        {this.renderTriggers()}
      </div>
    );
  }

  private onDrop(e: DragEvent) {
    const json = e.dataTransfer.getData('trigger-descriptor');
    const triggerDescriptor: TriggerDescriptor = JSON.parse(json);

    const trigger: Trigger = {
      id: uuid(),
      triggerType: triggerDescriptor.triggerType
    };

    this.triggers = [...this.triggers, trigger];
    this.triggerDescriptors = [...this.triggerDescriptors, triggerDescriptor];
  }

  private renderTriggers() {
    const triggers = this.triggers;
    const renderedTriggers = this.renderedTriggers;

    if (triggers.length == 0) {
      return <div>
        <div class="border-2 border-dashed m-4 p-4 text-center">
          <h2 class="text-lg font-medium text-gray-900">Triggers</h2>
          <p
            class="mt-1 text-sm text-gray-600">Place your workflow triggers here to automatically invoke your workflow when a trigger executes.</p>
        </div>
      </div>
    }

    return <div class="flex p-4 pt-8 space-x-4">
      {this.triggers.map((trigger, index) => {
        const html = renderedTriggers.get(trigger.id);
        return (
          <div class="flex-none">
            <div innerHTML={html}/>
          </div>
        );
      })}
    </div>
  }

}
