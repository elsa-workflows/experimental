import {Component, EventEmitter, h, Prop, State, Event, Method} from '@stencil/core';
import {v4 as uuid} from 'uuid';
import {Container} from 'typedi';
import {Activity, ActivitySelectedArgs, Trigger, TriggerDescriptor, Workflow} from '../../../models';
import {TriggerDriverRegistry} from '../../../services/trigger-driver-registry';
import WorkflowEditorTunnel from "../state";

export interface TriggersUpdatedArgs {
  triggers: Array<Trigger>;
  workflow: Workflow;
}

export interface TriggerSelectedArgs {
  trigger: Trigger;
  applyChanges: (trigger: Trigger) => void;
  deleteTrigger: (trigger: Trigger) => void;
}

export interface TriggerDeselectedArgs {
  trigger: Trigger;
}

@Component({
  tag: 'elsa-trigger-container',
})
export class TriggerContainer {
  private renderedTriggers: Map<string, string> = new Map<string, string>();

  @Prop({mutable: true}) public triggerDescriptors: Array<TriggerDescriptor> = [];
  @Prop({mutable: true}) public workflow: Workflow;
  @Event() public triggersUpdated: EventEmitter<TriggersUpdatedArgs>;
  @Event() triggerSelected: EventEmitter<TriggerSelectedArgs>;
  @Event() triggerDeselected: EventEmitter<TriggerDeselectedArgs>;
  @State() private triggers: Array<Trigger> = [];
  @State() private selectedTriggers: Array<Trigger> = [];

  @Method()
  public async deselectAll(): Promise<void> {
    this.selectedTriggers = [];
  }

  static onDragOver(e: DragEvent) {
    const isTrigger = e.dataTransfer.types.indexOf('trigger-descriptor') >= 0;

    if (isTrigger)
      e.preventDefault();
  }

  public componentWillRender() {
    const triggerDescriptors = this.triggerDescriptors;
    const triggers = this.workflow?.triggers || [];
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
           onDragOver={e => TriggerContainer.onDragOver(e)}
           onDrop={e => this.onDrop(e)}>
        {this.renderTriggers()}
      </div>
    );
  }

  private renderTriggers = () => {
    const triggers = this.triggers;

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
      {triggers.map(this.renderTrigger)}
    </div>
  };

  private renderTrigger = (trigger: Trigger) => {
    const renderedTriggers = this.renderedTriggers;
    const html = renderedTriggers.get(trigger.id);
    const isSelected = this.getIsTriggerSelected(trigger);
    const cssClass = isSelected ? 'border-green-400' : 'border-transparent';
    return (
      <div class={`p-1 border-2 border-dashed outline-none ${cssClass}`} tabindex={0}
           onKeyDown={e => this.onKeyPress(e)}>
        <div class="flex-none cursor-pointer select-none" onClick={e => this.onTriggerClick(e, trigger)}>
          <div innerHTML={html}/>
        </div>
      </div>
    );
  };

  private updateTriggers = (triggers: Array<Trigger>) => {
    this.workflow.triggers = triggers;
    this.triggers = triggers;
    this.triggersUpdated.emit({triggers: triggers, workflow: this.workflow});
  };

  private deleteTrigger = (trigger: Trigger) => {
    const triggers = this.triggers.filter(x => x != trigger);
    this.updateTriggers(triggers);
  };

  private updateTrigger = (trigger: Trigger) => this.updateTriggers([...this.triggers]);
  private getIsTriggerSelected = (trigger: Trigger) => this.selectedTriggers.findIndex(x => x == trigger) >= 0;

  private onDrop = (e: DragEvent) => {
    const json = e.dataTransfer.getData('trigger-descriptor');
    const triggerDescriptor: TriggerDescriptor = JSON.parse(json);

    const trigger: Trigger = {
      id: uuid(),
      triggerType: triggerDescriptor.triggerType
    };

    const triggers = [...this.workflow.triggers, trigger];
    this.updateTriggers(triggers);
  };

  private onTriggerClick = (e: MouseEvent, trigger: Trigger) => {
    const isSelected = this.getIsTriggerSelected(trigger);

    if (e.ctrlKey)
      this.selectedTriggers = isSelected ? this.selectedTriggers.filter(x => x != trigger) : [...this.selectedTriggers, trigger];
    else
      this.selectedTriggers = isSelected ? [] : [trigger];

    if (isSelected)
      this.triggerDeselected.emit({trigger});
    else
      this.triggerSelected.emit({
        trigger: trigger,
        applyChanges: this.updateTrigger,
        deleteTrigger: this.deleteTrigger
      });
  };

  private onKeyPress = (e: KeyboardEvent) => {
    if (e.key != 'Delete')
      return;

    const selectedTriggers = this.selectedTriggers;
    const triggers = this.triggers.filter(trigger => selectedTriggers.findIndex(x => x == trigger) < 0);
    this.updateTriggers(triggers);
  };
}

WorkflowEditorTunnel.injectProps(TriggerContainer, ['triggerDescriptors', 'workflow']);
